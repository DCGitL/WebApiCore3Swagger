using System;
using System.Collections.Generic;

namespace WebApiCore3Swagger.Health
{
    public class HealthCheckResponse
    {
        public string Status { get; set; }

        public DateTime TimeStamp { get; set; }

        public IEnumerable<HealthChecks> Checks { get; set; }

        public TimeSpan Duration { get; set; }

    }
}
