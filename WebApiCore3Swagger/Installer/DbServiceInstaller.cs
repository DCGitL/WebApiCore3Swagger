﻿using Adventure.Works._2012.dbContext.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestLib.InstallService;
using WebApiCore3Swagger.Models.IdentityDbContext;

namespace WebApiCore3Swagger.Installer
{
    public class DbServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {

            //add identity db context
            services.AddIdentityServiceExtension(configuration);

            //Northwind Db Service installer
            services.AddNorthwindDbService(configuration);

            //Adventure Db service installer
            services.AddAdventureWorksDbServices(configuration);
        }
    }
}