using MessageManager.RegisterSerive;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mime;
using WebApiCore3Swagger.Authentication.Basic;
using WebApiCore3Swagger.Authorizations;
using WebApiCore3Swagger.CustomRouteConstraint;
using WebApiCore3Swagger.Installer;
using WebApiCore3Swagger.Middleware;
using WebApiCore3Swagger.Middleware.JwtToken;
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


           


            //add mailing serivce
            services.AddMessageServices(Configuration);
            //mvc
            services.AddMvc(config =>
            {
                config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
            }).ConfigureApiBehaviorOptions(options =>
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
            }).AddNewtonsoftJson(); //Note add this package => Microsoft.AspNetCore.Mvc.NewtonsoftJson to the project this support the controller to return JsonResult 



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
                options.DefaultApiVersion = new ApiVersion(3, 2);
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

           // register middleware

            app.UseJwtTokenExpirationMiddleware();
         
            app.UseHeaderKeyToken();
            app.UseHttpsRedirection();

            app.UseRouting();

            

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v3.1/swagger.json", "v3.1");

                s.SwaggerEndpoint("/swagger/v2.2/swagger.json", "v2.2");
                s.RoutePrefix = string.Empty;
            });

            app.UseAuthentication();
            app.UseAuthorization();

          
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
