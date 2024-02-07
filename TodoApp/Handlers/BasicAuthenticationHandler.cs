using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using TodoApp.Contexts;
using TodoApp.Utils;

namespace TodoApp.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly TodoDBContext _context;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TodoDBContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {

            await base.HandleChallengeAsync(properties);
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            Response.ContentType = "text/plain";
            string message = "authentification failed you don't have access to this content";

            await Response.WriteAsync(message);

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Authorization header was not found");
            }

            try
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(":");
                string username = credentials[0];
                string password = credentials[1];
                var user = _context.Person.Include(e => e.Account).Include(e => e.Contact).SingleOrDefault(person => person.Account.Username == username && person.Account.Password == Md5.GennerateMD5(password));

                if (user == null)
                    return AuthenticateResult.Fail("Invalid username and password");
                else
                {
                    var claims = new[] { new Claim(ClaimTypes.Name, user.Account.Username) };
                    var indentify = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(indentify);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

                    return AuthenticateResult.Success(ticket);
                }
            }
            catch (Exception e)
            {
                return AuthenticateResult.Fail("Error has occured");
            }
        }
    }
}
