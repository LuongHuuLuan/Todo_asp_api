using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Common;
using TodoApp.Contexts;
using TodoApp.DTOs.AuthDTOs;
using TodoApp.DTOs.JWTAuthDTOs;
using TodoApp.DTOs.TodoDTOs.StatusDTO;
using TodoApp.Models.JWT;
using TodoApp.Models.Todos;
using TodoApp.Services;
using TodoApp.Utils;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthController : ControllerBase
    {
        private readonly TodoDBContext _context;
        private readonly IConfiguration _config;
        private readonly IJWTTokenServices _token;

        public JWTAuthController(TodoDBContext context, IConfiguration config, IJWTTokenServices token)
        {
            _context = context;
            _config = config;
            _token = token;
        }

        //[HttpPost("Login")]
        //public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO loginModel)
        //{
        //    // hash md5
        //    var encodeMD5Password = Md5.GennerateMD5(loginModel.Password);
        //    var user = _context.Person.Include(e => e.Account).Include(e => e.Contact).SingleOrDefault(person => person.Account.Username == loginModel.Username && person.Account.Password == Md5.GennerateMD5(loginModel.Password));

        //    if (user != null)
        //    {
        //        // generate token
        //        var key = _config["Jwt:Secret"];
        //        // mã hóa key
        //        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        //        // Ký vào signingKey
        //        var signingCredential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        //        // tạo claims chứa thông tin người dùng (Nếu cần)
        //        // Có thể cấu hình các role để bắt theo role new Claim(ClaimTypes.Role, "admin")
        //        var claims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name, user.LastName + "" + user.FirstName),
        //            new Claim(ClaimTypes.Role, "admin")
        //        };
        //        // tạo token với các thông số cấu hình
        //        var token = new JwtSecurityToken(
        //            issuer: _config["Jwt:Issuer"],
        //            audience: _config["Jwt:Audience"],
        //            expires: DateTime.Now.AddMinutes(10),
        //            signingCredentials: signingCredential,
        //            claims: claims
        //            );
        //        // sinh ra chuỗi token với các thông số trên
        //        var tokenGen = new JwtSecurityTokenHandler().WriteToken(token);

        //        return new JsonResult(new { username = user.Account.Username, token = tokenGen });
        //    }
        //    else
        //    {
        //        return new JsonResult(new { message = "Invalid username or password" });
        //    }
        //}

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticatedResponse>> Login([FromBody] LoginRequestDTO loginModel)
        {
            // hash md5
            var encodeMD5Password = Md5.GennerateMD5(loginModel.Password);
            var user = _context.Person.Include(e => e.Account).Include(e => e.Contact).SingleOrDefault(person => person.Account.Username == loginModel.Username && person.Account.Password == Md5.GennerateMD5(loginModel.Password));

            if (user != null)
            {
                // create claims for token
                var jti = Guid.NewGuid().ToString();
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.LastName + "" + user.FirstName),
                    new Claim(ClaimTypes.Role, "admin"),
                    new Claim(JwtRegisteredClaimNames.Jti, jti)
                };

                var accessToken = _token.GenerateAccessToken(claims);
                var refreshToken = _token.GenerateRefreshToken(claims);

                // get expire time of token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(refreshToken);
                var expires = jwtToken.ValidTo;

                // save token to db
                var outStandingToken = new OutstandingToken
                {
                    Jti = jti,
                    Token = refreshToken,
                    ExpiresAt = expires,
                    CreatedAt = DateTime.Now,
                    Account = user.Account
                };

                _context.OutstandingTokens.Add(outStandingToken);
                await _context.SaveChangesAsync();

                return new AuthenticatedResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
            }
            else
            {
                return new JsonResult(new { message = "Invalid username or password" });
            }
        }

        [HttpPost("Refresh")]
        public async Task<ActionResult<AuthenticatedResponse>> Refresh([FromBody] RefreshTokenRequest refreshTokenModel)
        {
            if (refreshTokenModel is null)
                return BadRequest();

            var principal = _token.GetPrincipalFromExpiredToken(refreshTokenModel.RefreshToken, IJWTTokenServices.JWTTokenType.Refresh);
            //var username = principal.Identity.Name;
            // Check token in blacklist
            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            var blacklistedToken = await _context.BlacklistedTokens
                                    .Include(bt => bt.Token)
                                    .SingleOrDefaultAsync(bt => bt.Token.Jti == jti.Value);
            if (blacklistedToken != null)
                return BadRequest("Token in blacklist");

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var jwtToken = tokenHandler.ReadJwtToken(refreshTokenModel.RefreshToken);
            //var expires = jwtToken.ValidTo;
            var outstandingToken = await _context.OutstandingTokens.SingleOrDefaultAsync(e => e.Jti == jti.Value);

            if (outstandingToken.ExpiresAt <= DateTime.Now)
                return BadRequest("Invalid client request");

            var newAccessToken = _token.GenerateAccessToken(principal.Claims);
            var newRefreshToken = refreshTokenModel.RefreshToken;

            return new AuthenticatedResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest refreshTokenModel)
        {
            if (refreshTokenModel is null)
                return BadRequest();

            var principal = _token.GetPrincipalFromExpiredToken(refreshTokenModel.RefreshToken, IJWTTokenServices.JWTTokenType.Refresh);
            //var username = principal.Identity.Name;
            // Check token in blacklist
            var jti = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
            var blacklistedToken = await _context.BlacklistedTokens
                                    .Include(bt => bt.Token)
                                    .SingleOrDefaultAsync(bt => bt.Token.Jti == jti.Value);
            if (blacklistedToken != null)
                return BadRequest("Token already in blacklist");

            var outstandingToken = await _context.OutstandingTokens.SingleOrDefaultAsync(e => e.Jti == jti.Value);

            _context.BlacklistedTokens.Add(new BlacklistedToken
            {
                BlacklistedAt = DateTime.Now,
                Token = outstandingToken
            });;

            _context.SaveChanges();

            return NoContent();
        }
    }

}
