using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Contexts;
using TodoApp.DTOs.AuthDTOs;
using TodoApp.Models.JWT;
using TodoApp.Utils;

namespace TodoApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTAuthController
    {
        private readonly TodoDBContext _context;
        private readonly IConfiguration _config;

        public JWTAuthController(TodoDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequestDTO loginModel)
        {
            // hash md5
            var encodeMD5Password = Md5.GennerateMD5(loginModel.Password);
            var user = _context.Person.Include(e => e.Account).Include(e => e.Contact).SingleOrDefault(person => person.Account.Username == loginModel.Username && person.Account.Password == Md5.GennerateMD5(loginModel.Password));

            if (user != null)
            {
                // generate token
                var key = _config["Jwt:Secret"];
                // mã hóa key
                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                // Ký vào signingKey
                var signingCredential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                // tạo claims chứa thông tin người dùng (Nếu cần)
                // Có thể cấu hình các role để bắt theo role new Claim(ClaimTypes.Role, "admin")
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.LastName + "" + user.FirstName),
                    new Claim(ClaimTypes.Role, "admin")
                };
                // tạo token với các thông số cấu hình
                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:ValidIssuer"],
                    audience: _config["Jwt:ValidAudience"],
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signingCredential,
                    claims: claims
                    );
                // sinh ra chuỗi token với các thông số trên
                var tokenGen = new JwtSecurityTokenHandler().WriteToken(token);

                return new JsonResult(new { username = user.Account.Username, token = tokenGen });
            }
            else
            {
                return new JsonResult(new { message = "Invalid username or password" });
            }
        }
    }
}
