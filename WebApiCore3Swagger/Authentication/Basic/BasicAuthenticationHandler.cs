using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.Auth;
using WebApiCore3Swagger.Models.IdentityDbContext;

namespace WebApiCore3Swagger.Authentication.Basic
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly UserManager<ApplicationUser> userManager;

        public BasicAuthenticationHandler( IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, UserManager<ApplicationUser> userManager) : base(options,logger,encoder, clock)
        {
           
            this.userManager = userManager;
        }
        protected override  async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing authorization header value");
            }
           
            ApplicationUser appUser = null;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
                var username = credentials[0];
                var password = credentials[1];
                bool isUserValid = false;
                appUser = await userManager.FindByNameAsync(username);
                if (appUser != null && await userManager.CheckPasswordAsync(appUser, password))
                {
                    isUserValid = true;
                }

                if (!isUserValid)
                {
                    return AuthenticateResult.Fail("Invalid user"); 
                }
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid Authorization header");
            }

            var appUserRoles = await userManager.GetRolesAsync(appUser);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName),
            };

            if (appUserRoles != null && appUserRoles.Count > 0)
            {
                foreach (var role in appUserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
