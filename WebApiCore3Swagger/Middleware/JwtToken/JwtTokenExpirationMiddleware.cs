using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace WebApiCore3Swagger.Middleware.JwtToken
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class JwtTokenExpirationMiddleware
    {
        private readonly RequestDelegate _next;
        private string invalidTokenMsg = string.Empty;
        public JwtTokenExpirationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IHeaderDictionary headers = httpContext.Request.Headers;
            if (headers.TryGetValue("Authorization", out StringValues authValues))
            {
                if (authValues[0].Contains("Bearer"))
                {
                    string[] tokenval = authValues[0].Split(" ".ToArray());
                    if (tokenval.Length == 2)
                    {
                        var jwttokenParameters = httpContext.RequestServices.GetRequiredService<TokenValidationParameters>();
                
                        var jwtToken = tokenval[1];
                        bool isTokenExpired = ValidateJwtTokenExpirationTime(jwttokenParameters, jwtToken);
                        if (isTokenExpired)
                        {
                            var tokenmsg = "This token is expired";
                            if (!String.IsNullOrEmpty(invalidTokenMsg))
                            {
                                tokenmsg = invalidTokenMsg;
                            }
                            httpContext.Response.Headers.Add("TokenExpire", tokenmsg);
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            return;
                        }
                    };
                }
            }

            await _next(httpContext);
        }


        private bool ValidateJwtTokenExpirationTime(TokenValidationParameters tokenParameters, string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            try
            {

                var claimsPrincipal = tokenHandler.ValidateToken(token, tokenParameters, out var _validatedToken);

                var exptime = claimsPrincipal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value;

                var expiryDateUnix = long.Parse(exptime);

                var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                           .AddSeconds(expiryDateUnix);


                if (expiryDateTimeUtc > DateTime.UtcNow)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                invalidTokenMsg = "This token is invalid";
                return false;
            }
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtTokenExpirationMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtTokenExpirationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtTokenExpirationMiddleware>();
        }
    }
}
