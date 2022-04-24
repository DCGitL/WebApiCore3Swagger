using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeDB.Dal.HealthCheck
{
    public class EmployeeDbHealthCheck : IHealthCheck
    {
        private readonly string _sqlconnectionstr;
        public EmployeeDbHealthCheck(IConfiguration configuration)
        {
            _sqlconnectionstr = configuration.GetConnectionString("EmployeeDB");
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (SqlConnection conn = new SqlConnection(_sqlconnectionstr))
            {
                try
                {
                    conn.Open();
                    var healthResult = HealthCheckResult.Healthy("Employee db is available");
                    return await Task.FromResult(healthResult);

                }
                catch (Exception ex)
                {
                    var healthResult = HealthCheckResult.Unhealthy("Employee db is available", ex);
                    return await Task.FromResult(healthResult);


                }
            }
        }
    }
}
