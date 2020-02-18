using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WebApiCore3Swagger.SwaggerFilters;

namespace WebApiCore3Swagger.Installer
{
    public class SwaggerServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            //swagger
            services.AddSwaggerGen(g =>
            {
                g.SwaggerDoc("v3.1", new OpenApiInfo { Title = "v3.1 Core web api", Description = "Swagger core api 3.1", Version = "v3.1" });

                g.SwaggerDoc("v2.2", new OpenApiInfo { Title = "v2.2 Core web api", Description = "Swagger core api 2.2", Version = "v2.2" });

               
                g.OperationFilter<RemoveVersionFromParameter>();
                g.DocumentFilter<ReplaceVersionWithValueInPath>();

                // g.OperationFilter<AuthenticationHeaderOperationFilter>();

                var bearerSecurityScheme = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please add Bearer with a speace before the key in the below text box => Bearer Z29vZEtleQ==",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"


                };
                g.AddSecurityDefinition("Bearer", bearerSecurityScheme);

                var bearerSecurityRequirement = new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",

                            }

                        },
                        new string[]{}

                    } };
                g.AddSecurityRequirement(bearerSecurityRequirement);




                var basicSecurityScheme = new OpenApiSecurityScheme
                {

                    In = ParameterLocation.Header,
                    Description = "Please add Basic with a speace before the key in the below text box Note username:pasword must be separated by a d colan then 64 bit encoded before sending => Basic Z29vZEtleQ==",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Basic"


                };
                g.AddSecurityDefinition("Basic", basicSecurityScheme);
                var basicSecurityRequirement = new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }

                        },
                        new string[]{}
                    } };
                g.AddSecurityRequirement(basicSecurityRequirement);

                
                g.DocInclusionPredicate((version, desc) =>
                {
                    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                        return false;
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true)
                    .OfType<ApiVersionAttribute>()
                    .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v.ToString()}" == version);
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                g.IncludeXmlComments(xmlPath);

            });
        }
    }
}
