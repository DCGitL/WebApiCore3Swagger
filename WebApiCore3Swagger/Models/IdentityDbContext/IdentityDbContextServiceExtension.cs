using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApiCore3Swagger.Models.IdentityDbContext
{
    public static class IdentityDbContextServiceExtension
    {
        public static void AddIdentityServiceExtension(this IServiceCollection services, IConfiguration configuration)

        {
            var connectionstring = configuration.GetConnectionString("EmployeeDbStore");
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(connectionstring));
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 3;
            }).AddEntityFrameworkStores<AppIdentityDbContext>();

        }
    }
}
