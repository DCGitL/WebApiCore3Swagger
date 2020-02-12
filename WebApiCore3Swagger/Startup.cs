using Adventure.Works._2012.dbContext.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Mime;
using WebApiCore3Swagger.Authentication.Basic;
using WebApiCore3Swagger.Installer;
using WebApiCore3Swagger.Middleware.JwtToken;
using WebApiCore3Swagger.Models.IdentityDbContext;
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

            //add identity db context
            services.AddIdentityServiceExtension(Configuration);

            //Adventure Works Db Service
            services.AddNorthwindDbService(Configuration);

            // services.AddBasicAuthenticationService();  //==>Note this set basic authentication globally


            // [Authorize(AuthenticationSchemes = "BasicAuthentication")] this set the authentication at the controller level
            services.AddAuthentication("BasicAuthentication")
           .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            //Jwt authentication  service Registration
            services.AddJwtAuthServiceConfiguration(Configuration);
            services.AddAuthServiceExtension(Configuration);

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
            });

            //service installation goes here

            services.AddServicesInstaller(Configuration);


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

            app.UseJwtTokenExpirationMiddleware();

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
