using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data.Common;
using System.Data;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace WebApiCore3Swagger.Health.Datatabase
{
    public class AdventureWorkDbHealthCheck :IHealthCheck
    {
        private readonly IConfiguration _configuration;
        private const string sqlQuery = "select 1 as val";

        public AdventureWorkDbHealthCheck(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connectionStr = _configuration.GetConnectionString("AdventureWorks");
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
                        return HealthCheckResult.Healthy("AdventureWorks Db is available");
                    }
                }
            }
            catch (DbException ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus,
                    description: "Fail to access AdventureWorks Db",
                    exception: ex);

            }


        }
    }
}
