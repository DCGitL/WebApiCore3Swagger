using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace WebApiCore3Swagger.Health.Datatabase
{
    public class EmployeeDbHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        private const string sqlQuery = "select 1 as val";

        public EmployeeDbHealthCheck(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connectionStr = _configuration.GetConnectionString("EmployeeDB");
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = sqlQuery;
                        cmd.CommandType = CommandType.Text;
                        if (cmd.Connection.State == ConnectionState.Closed)
                        {
                            await cmd.Connection.OpenAsync();
                        }
                        await cmd.ExecuteNonQueryAsync();
                        return HealthCheckResult.Healthy("EmployeeDB is available");
                    }
                }
            }
            catch (DbException ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus,
                    description: "Fail to access EmployeeDB",
                    exception: ex);

            }


        }
    }
}
