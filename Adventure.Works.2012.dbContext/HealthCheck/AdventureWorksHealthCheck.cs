
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure.Works._2012.dbContext.HealthCheck
{
    public class AdventureWorksHealthCheck : IHealthCheck
    {
        private readonly string _sqlconnectionstr;
        public AdventureWorksHealthCheck(IConfiguration configuration)
        {
            _sqlconnectionstr = configuration.GetConnectionString("AdventureWorks");
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
           using (SqlConnection conn = new SqlConnection(_sqlconnectionstr))
            {
                try
                {
                    conn.Open();
                    var healthResult = HealthCheckResult.Healthy("Adventure db is available");
                    return await Task.FromResult(healthResult);

                }
                catch (Exception ex)
                {
                    var healthResult = HealthCheckResult.Unhealthy("Adventure db is available", ex);
                    return await Task.FromResult(healthResult);


                }
            }
        }
    }
}
