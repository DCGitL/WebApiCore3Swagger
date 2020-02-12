using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiCore3Swagger.Authentication.Basic
{
    public static class BasicAuthenticationServiceExtension
    {
        public static void AddBasicAuthenticationService(this IServiceCollection services)
        {
            //basic authentication service registration if options is not net here as the default scheme it 
            // can be set at the controller level as a Authorize attribute => example  [Authorize(AuthenticationSchemes = "BasicAuthentication")]
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = "BasicAuthentication";
                options.DefaultChallengeScheme = "BasicAuthentication";
            })
             .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

        }
    }
}
