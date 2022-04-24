using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Redis.HealthCheck
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public RedisHealthCheck(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer=connectionMultiplexer;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var database = _connectionMultiplexer.GetDatabase();
                database.StringGet(key: "healthy");
                var healthResult = HealthCheckResult.Healthy("Available");
                return await Task.FromResult(healthResult);
            }
            catch (Exception ex)
            {
                var healthResult = HealthCheckResult.Unhealthy("Not available", ex);
                return await Task.FromResult(healthResult);
            }
        }
    }
}
