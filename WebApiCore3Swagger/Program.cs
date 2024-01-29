using MessageManager.RegisterSerive;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Collections.Generic;
using System.Net.Mime;
using WebApiCore3Swagger.Authentication.Basic;
using WebApiCore3Swagger.Authorizations;
using WebApiCore3Swagger.CustomRouteConstraint;
using WebApiCore3Swagger.Health.ServiceExtensions;
using WebApiCore3Swagger.Installer;
using WebApiCore3Swagger.Middleware;
using WebApiCore3Swagger.Middleware.JwtToken;
using WebApiCore3Swagger.Services.Auth.ServiceExtension;

var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseNLog();

var configuration = builder.Configuration;
var MyCorsPolicy = "CorsPolicy";
var services = builder.Services;
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

services.AddControllers(options =>
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

services.AddServicesInstaller(configuration);
// services.AddBasicAuthenticationService();  //==>Note this set basic authentication globally


// [Authorize(AuthenticationSchemes = "BasicAuthentication")] this set the authentication at the controller level
services.AddAuthentication("BasicAuthentication")
.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

//Jwt authentication  service Registration
services.AddJwtAuthServiceConfiguration(configuration);
services.AddAuthServiceExtension(configuration);
//Adding custom requirement into our custom policy
services.AddAuthorization(options =>
{
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
services.AddMessageServices(configuration);
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

var app = builder.Build();

app.UseCors(MyCorsPolicy);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(u =>
    {
        u.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
        {
            swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}", Description = app.Environment.EnvironmentName } };
            swaggerDoc.Extensions["x-cmdbid"] = new OpenApiString("2034");
        });

    });
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v3.1/swagger.json", "v3.1");

        s.SwaggerEndpoint("/swagger/v2.2/swagger.json", "v2.2");
        s.RoutePrefix = string.Empty;
    });


    app.UseExceptionHandler("/error-local-development");


}
else
{
    app.UseExceptionHandler("/error");
}

app.UseJwtTokenExpirationMiddleware();

app.UseHeaderKeyToken();
app.UseHttpsRedirection();

app.UseRouting();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecksUI();
//Global error handler 

app.MapControllers();
//app.MapHealthChecks("/health");

app.Run();

