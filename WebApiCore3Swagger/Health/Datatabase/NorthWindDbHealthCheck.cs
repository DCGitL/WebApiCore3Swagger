
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Health.Datatabase
{
    public class NorthWindDbHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;
        private const string sqlQuery = "select 1 as val";

        public NorthWindDbHealthCheck(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var connectionStr = _configuration.GetConnectionString("Northwindb");
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
                        return HealthCheckResult.Healthy("Northwind db is available");
                    }
                } 
            }
            catch (DbException  ex)
            {
                return new HealthCheckResult(status: context.Registration.FailureStatus,
                    description: "Fail to access northwind db",
                    exception: ex);
                
            }
           
           
        }
    }
}
