using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestLib.AdventureWorks.Repository;
using TestLib.Context;

namespace TestLib.InstallService
{
    public static class AdventureWorksServicesExtension
    {

        public static void AddAdventureWorksDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AdventureWorksDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AdventureWorks")));

            services.AddTransient<IAdventureWorksRepository, AdventureWorksRepository>();
        }
    }
}
