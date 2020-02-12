using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace WebApiCore3Swagger.Installer
{
    public static class ServicesInstallerExtension
    {
        public static void AddServicesInstaller(this IServiceCollection services, IConfiguration configuration)
        {
            var servicesInstallers = typeof(Startup).Assembly.ExportedTypes.Where(t => typeof(IServiceInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract).Select(Activator.CreateInstance).Cast<IServiceInstaller>().ToList();

            servicesInstallers.ForEach(serviceInstaller => serviceInstaller.Install(services, configuration));
        }
    }
}
