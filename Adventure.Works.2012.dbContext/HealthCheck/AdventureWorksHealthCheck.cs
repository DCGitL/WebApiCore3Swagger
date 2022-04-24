
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Adventure.Works._2012.dbContext.HealthCheck
{
    public class AdventureWorksHealthCheck : IHealthCheck
    {
        private readonly string _sqlconnectionstr;
        private const string testQuery = "Select 1 as val";
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
                    await conn.OpenAsync(cancellationToken);
                    var command = conn.CreateCommand();
                    command.CommandText = testQuery;
                    await command.ExecuteNonQueryAsync();

                    var heathResult = HealthCheckResult.Healthy("AdventureWorks Db is available");
                    return heathResult;
                }
                catch (DbException ex)
                {
                    var heathResult = new HealthCheckResult(status: context.Registration.FailureStatus,
                        description: "Fail to access AdventureWorks db",
                        exception: ex);
                    return heathResult;
                }
            }
        }
    }
}
