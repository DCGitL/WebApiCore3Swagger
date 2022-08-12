
using AutoMapper;
using EmployeeDB.Dal.Employee.DbRepository;
using EmployeeDB.Dal.EmployeeDbAutoMapper;
using EmployeeDB.Dal.HealthCheck;
using EmployeeDB.Dal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmployeeDB.Dal.Service
{
    public static class EmloyeeDbServiceExtension
    {
        public static void AddEmployeeDbExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("EmployeeDB");
            services.AddDbContext<EmployeeDbContext>(options => options.UseSqlServer(connectionstring));
            services.AddScoped<IEmployeeDbRepository, EmployeeDbRepository>();
          
            //auto mapper configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new EmployeeDbAutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
