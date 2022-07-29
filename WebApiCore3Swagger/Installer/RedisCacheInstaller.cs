using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WebApiCore3Swagger.Redis.HealthCheck;
using WebApiCore3Swagger.RedisCache.Attribs.Settings;
using WebApiCore3Swagger.RedisCacheServices;

namespace WebApiCore3Swagger.Installer
{
    public class RedisCacheInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(key: nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);
            if(!redisCacheSettings.Enabled)
            {
                return;
            }

            services.AddSingleton<IConnectionMultiplexer>(_=> ConnectionMultiplexer.Connect(redisCacheSettings.ConnectionString));

            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseRedisCacheServices>();
            services.AddHealthChecks()
                .AddCheck<RedisHealthCheck>(name: "Redis");
             
        }
    }
}
