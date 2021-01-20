using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
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
            var publiccertificatepath = appsetting.PublicKeyLocation;
            var certificate = new X509Certificate2(publiccertificatepath);

            var key = new X509SecurityKey(certificate);  // Encoding.ASCII.GetBytes(appsetting.JwtTokenSecret);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key, // new SymmetricSecurityKey(key),
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
           .AddJwtBearer(configOptions =>
           {
               configOptions.Events = new JwtBearerEvents()
               {
                   OnMessageReceived = context =>
                   {
                       StringValues token;
                       //extract the access token from the querystring if it is available
                       if (context.Request.Query.ContainsKey("access_token"))
                       {
                           context.Token = context.Request.Query["access_token"];
                       }

                       if (context.Request.Headers.TryGetValue("Authorization", out token))
                       {
                           context.Token = token.ToString().Replace("Bearer ", "");
                       }

                       return Task.CompletedTask;
                   }

               };

               configOptions.RequireHttpsMetadata = false;
               configOptions.SaveToken = true;
               configOptions.TokenValidationParameters = tokenValidationParameters;
           });

            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser().Build();
            //});
        }
    }
}
