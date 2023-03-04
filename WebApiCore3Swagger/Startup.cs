using HealthChecks.UI.Client;
using MessageManager.RegisterSerive;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using WebApiCore3Swagger.Authentication.Basic;
using WebApiCore3Swagger.Authorizations;
using WebApiCore3Swagger.CustomRouteConstraint;
using WebApiCore3Swagger.Health.ServiceExtensions;
using WebApiCore3Swagger.Installer;
using WebApiCore3Swagger.Middleware;
using WebApiCore3Swagger.Middleware.JwtToken;
using WebApiCore3Swagger.Models.HealthCheck;
using WebApiCore3Swagger.Services.Auth.ServiceExtension;


namespace WebApiCore3Swagger
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyCorsPolicy = "CorsPolicy";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyCorsPolicy, builder =>
                {
                    builder//.WithOrigins("http://localhost:5000")
                    .SetIsOriginAllowed(so => true)
                    .WithExposedHeaders("Content-Disposition")
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .AllowCredentials();
                });

            });
            services.AddControllers( options =>
            {
                options.RespectBrowserAcceptHeader= true;
                options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            })
                .AddNewtonsoftJson()          
              .ConfigureApiBehaviorOptions(options =>
              {
                  //Global validation on posted objects with dataanotations.
                  options.InvalidModelStateResponseFactory = context =>
                  {
                      var result = new BadRequestObjectResult(context.ModelState);

                      // TODO: add `using using System.Net.Mime;` to resolve MediaTypeNames
                      result.ContentTypes.Add(MediaTypeNames.Application.Json);
                      result.ContentTypes.Add(MediaTypeNames.Application.Xml);

                      return result;

                  };
              });
            // services.AddControllers();//.AddXmlSerializerFormatters(); => this was move to the installer



            //service installation goes here

            services.AddServicesInstaller(Configuration);

            // services.AddBasicAuthenticationService();  //==>Note this set basic authentication globally


            // [Authorize(AuthenticationSchemes = "BasicAuthentication")] this set the authentication at the controller level
            services.AddAuthentication("BasicAuthentication")
           .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Jwt authentication  service Registration
            services.AddJwtAuthServiceConfiguration(Configuration);
            services.AddAuthServiceExtension(Configuration);

            //Adding custom requirement into our custom policy
            services.AddAuthorization(options => {
                //Implement this by adding this => [Authorize(Policy="MustWorkForDavidCom")] at the controller or required end point
                options.AddPolicy("MustWorkForDavidcom", policy =>
                {
                    policy.AddRequirements(new WorksForCompanyRequirement("david.com"));
                    
                });
                //Implement this by adding this => [Authorize(Policy="MustBedavidcomandAdmin")] at the controller or required end point
                options.AddPolicy("MustBedavidcomandAdmin", policy =>
                {
                    policy.AddRequirements(new CustomizedAuthorizationRequirement());
                });
            });
            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();

            //Note add the customAuthorizationHandler as AddTransient if injecting database objects this will cause a database call everytime when user hits the end point that uses this authorization policy
            services.AddTransient<IAuthorizationHandler, CustomizedAuthorizationHandler>();
            //add my custom authorizaton policy

            services.AddHealthCheckServices();



            //add mailing serivce
            services.AddMessageServices(Configuration);
            //mvc
          
          //Note add this package => Microsoft.AspNetCore.Mvc.NewtonsoftJson to the project this support the controller to return JsonResult 



            //add custom routing constraints. Note the key LatLongContraint is add to the route at the end point
            services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("LatLongContraint", typeof(CustomRouteContraintOnParameterTypeDouble));
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                });

                
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
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseCors(MyCorsPolicy);

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            // setup health check pipeline endpoint
            #region healthcheckJsonoutputUsingHealthUIInstead
            //app.UseHealthChecks("/health", new HealthCheckOptions()
            //{
            //    ResponseWriter = async (context, report) =>
            //    {
            //        context.Response.ContentType = "application/json";
            //        var response = new HealthCheckResponse()
            //        {
            //            Status = report.Status.ToString(),
            //            Checks = report.Entries.Select(e => new HealthCheck()
            //            {
            //                Component = e.Key,
            //                Status = e.Value.Status.ToString(),
            //                Description = e.Value.Description,
            //                ExceptionMessage = e.Value.Exception != null ? e.Value.Exception.Message : "none"

            //            }),
            //            Duration = report.TotalDuration
            //        };

            //        await context.Response.WriteAsync(text: JsonConvert.SerializeObject(response, Formatting.Indented));
            //    }

            //});
            #endregion  healthcheckJsonoutputUsingHealthUIInstead
            // register middleware

            app.UseJwtTokenExpirationMiddleware();
         
            app.UseHeaderKeyToken();
            app.UseHttpsRedirection();

            app.UseRouting();

            

            app.UseSwagger(u => {
                u.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}", Description = env.EnvironmentName } };
                    swaggerDoc.Extensions["x-cmdbid"] = new OpenApiString("2034");
                });
            
            });
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v3.1/swagger.json", "v3.1");

                s.SwaggerEndpoint("/swagger/v2.2/swagger.json", "v2.2");
                s.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseAuthorization();
            //this end point is use for health check UI
            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse

            });
          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecksUI();
                endpoints.MapControllers();
            });


        }
    }
}
