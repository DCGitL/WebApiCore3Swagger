using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.HealthCheck;

namespace WebApiCore3Swagger.Controllers.Health
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.2")]
    public class HealthController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(HealthCheckService healthCheckService, ILogger<HealthController> logger)
        {
            _healthCheckService=healthCheckService;
            _logger=logger;
        }
        [HttpGet("GetApiHealth",Name ="ApiHealthStatus")]
        [MapToApiVersion("2.2")]
        [ProducesResponseType(StatusCodes.Status200OK,Type = typeof(HealthCheckResponse))]
        public async Task<IActionResult> GetApiHealthStatus()
        {
            var healthReport = await _healthCheckService.CheckHealthAsync();

            var healthResponse = new HealthCheckResponse
            {
                Status = healthReport.Status.ToString(),
                Checks = healthReport.Entries.Select(e =>
                   new HealthCheck
                    {
                        Status = e.Value.Status.ToString(),
                        Component = e.Key,
                        Description = e.Value.Description?? string.Empty,
                        ExceptionMessage = e.Value.Exception?.Message ?? string.Empty,
                        Data = e.Value.Data,
                        Duration = e.Value.Duration

                    }),
                Duration = healthReport.TotalDuration,

            }; 

            return Ok(healthResponse);

        }
    }
}
