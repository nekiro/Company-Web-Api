using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace CompanyApp.Models
{
    public class BasicAuthOptions : AuthenticationSchemeOptions
    {
        public BasicAuthOptions()
        {
        }
    }

    public class BasicAuthHandler : AuthenticationHandler<BasicAuthOptions>
    {
        public IServiceProvider ServiceProvider { get; set; }

        public BasicAuthHandler(IOptionsMonitor<BasicAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string authHeader = Request.Headers["Authorization"];

            if (authHeader == null || !authHeader.StartsWith("Basic"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing authorization header"));
            }

            string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
            try
            {
                string decodedCredentials = Encoding.GetEncoding("UTF-8").GetString(Convert.FromBase64String(encodedCredentials));
                int seperatorIndex = decodedCredentials.IndexOf(':');

                var username = decodedCredentials.Substring(0, seperatorIndex);
                var password = decodedCredentials.Substring(seperatorIndex + 1);
                
                if (!(username == "admin" && password == "admin"))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Wrong credentials"));
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                };

                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                var ticket = new AuthenticationTicket(claimsPrincipal,
                    new AuthenticationProperties { IsPersistent = false },
                    Scheme.Name
                );

                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (FormatException) {}

            return Task.FromResult(AuthenticateResult.Fail("Missing authorization header"));
        }
    }
}
