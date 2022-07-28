using Microsoft.Extensions.DependencyInjection;
using WebApiCore3Swagger.Health.Datatabase;

namespace WebApiCore3Swagger.Health.ServiceExtensions
{
    public static class HealthServiceExtension
    {
        public static IServiceCollection AddHealthCheckServices(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddCheck<NorthWindDbHealthCheck>(name:"NorthwindDb",tags: new[] {"database"})
                .AddCheck<EmployeeDbHealthCheck>(name:"EmployeDb", tags: new[] {"database"} )
                .AddCheck<AdventureWorkDbHealthCheck>(name: "AdventureWorks", tags: new[] { "database" });
           

            return services;
        }
    }
}
