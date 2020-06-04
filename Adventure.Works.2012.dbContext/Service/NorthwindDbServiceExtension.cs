using Adventure.Works._2012.dbContext.Models;
using Adventure.Works._2012.dbContext.Northwind.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Adventure.Works._2012.dbContext.Service
{
    public static class NorthwindDbServiceExtension
    {
        public static void AddNorthwindDbService(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("Northwindb");
            services.AddDbContext<NorthwindContext>(options => options.UseSqlServer(connectionstring));

            services.AddScoped<INorthwindRepository, NorthwindRepository>();

           
        }
    }
}
