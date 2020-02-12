using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Services.Auth.ServiceExtension
{
    public static class AddJwtServiceExtension
    {
        public static void AddJwtAuthServiceConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtsecret = configuration.GetSection("AppSetting");
            var appsetting = jwtsecret.Get<AppSetting>();
            var key = Encoding.ASCII.GetBytes(appsetting.JwtTokenSecret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                AuthenticationType = "Bearer"

            };

            //register the jwt token parameter into the services container to get it whenever I need it 
            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(x =>
           {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = tokenValidationParameters;
           });
        }
    }
}
