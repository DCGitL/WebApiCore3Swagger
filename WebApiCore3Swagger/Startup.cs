using Adventure.Works._2012.dbContext.AutoMapper;
using AutoMapper;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();//.AddXmlSerializerFormatters();


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

            //Adding Cross Origin Resource Sharing (CORS) 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials()
                    .SetIsOriginAllowed((host) => true));
            });


            //auto mapper configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperDataProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);


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


            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(3, 2);
                options.AssumeDefaultVersionWhenUnspecified = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

           

            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            // global cors policy make sure it is applied before Usemvc
            app.UseCors("CorsPolicy");

            //  app.UseJwtTokenExpirationMiddleware();

            app.UseHttpsRedirection();

            app.UseRouting();



            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v3.1/swagger.json", "v3.1");

                s.SwaggerEndpoint("/swagger/v2.2/swagger.json", "v2.2");
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
