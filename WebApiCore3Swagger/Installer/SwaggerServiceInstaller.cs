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



            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(2, 2);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddEndpointsApiExplorer();
            //swagger
            services.AddSwaggerGen(g =>
            {
                var contact = new OpenApiContact
                {
                    Email = "david.chen@gmail.com",
                    Name = "David",
                    Url = new Uri("http://www.goole.com")
                };
                g.SwaggerDoc("v3.1", new OpenApiInfo {
                    Title = "v3.1 Core web api", Description = "Swagger core api 3.1", Version = "v3.1", Contact = contact });

                g.SwaggerDoc("v2.2", new OpenApiInfo { 
                    Title = "v2.2 Core web api", Description = "Swagger core api 2.2", Version = "v2.2", Contact = contact });


                //g.OperationFilter<RemoveVersionFromParameter>();
                //g.DocumentFilter<ReplaceVersionWithValueInPath>();

                // g.OperationFilter<AuthenticationHeaderOperationFilter>();

                //Jwt bearer Token
                string name = "Authorization";
                string schemeName ="Bearer";
                string description = "Please add Bearer with a space before the key in the below text box => Bearer Z29vZEtleQ==";
                var SecurityScheme = GetSecuritySchema(name, schemeName, description);
                g.AddSecurityDefinition(schemeName, SecurityScheme);
                var SecurityRequirement = GetSecurityRequirement(schemeName);
                g.AddSecurityRequirement(SecurityRequirement);


                schemeName = "Basic";
                description = "Please add Basic with a speace before the key in the below text box Note username:pasword must be separated by a d colan then 64 bit encoded before sending => Basic Z29vZEtleQ==";
                SecurityScheme = GetSecuritySchema(name, schemeName, description);
                g.AddSecurityDefinition(schemeName, SecurityScheme);
                SecurityRequirement = GetSecurityRequirement(schemeName);
                g.AddSecurityRequirement(SecurityRequirement);

                
                //g.DocInclusionPredicate((version, desc) =>
                //{
                //    if (!desc.TryGetMethodInfo(out MethodInfo methodInfo))
                //        return false;
                //    var versions = methodInfo.DeclaringType.GetCustomAttributes(true)
                //    .OfType<ApiVersionAttribute>()
                //    .SelectMany(attr => attr.Versions);

                //    return versions.Any(v => $"v{v.ToString()}" == version);
                //});

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                g.IncludeXmlComments(xmlPath);

            });
        }



        private OpenApiSecurityScheme GetSecuritySchema(string name, string schemeName, string description )
        {
            return new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description =  description,
                Name = name, 
                Type = SecuritySchemeType.ApiKey,
                Scheme = schemeName 


            };
        }

        private OpenApiSecurityRequirement GetSecurityRequirement(string id)
        {
            return new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = id,

                            }

                        },
                        new string[]{}

                    } };
        }
    }
}
