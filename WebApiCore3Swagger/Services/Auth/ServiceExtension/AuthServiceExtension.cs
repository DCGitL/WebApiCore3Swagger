using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Services.Auth.ServiceExtension
{
    public static class AuthServiceExtension
    {
        public static void AddAuthServiceExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var appconfig = configuration.GetSection("AppSetting");
            var result = appconfig.Get<AppSetting>();
            services.Configure<AppSetting>(options =>
            {
                options.JwtTokenSecret = result.JwtTokenSecret;
                options.TokenLifeTime = result.TokenLifeTime;
                options.PrivateKeyLocation = result.PrivateKeyLocation;
                options.PublicKeyLocation = result.PublicKeyLocation;
            });


            services.AddScoped<IAuthJwtService, AuthJwtService>();

        }
    }
}

