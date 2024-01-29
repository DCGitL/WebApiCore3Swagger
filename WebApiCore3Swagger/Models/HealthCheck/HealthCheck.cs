using System;
using System.Collections.Generic;

namespace WebApiCore3Swagger.Models.HealthCheck
{
    public class HealthCheck
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
        public string ExceptionMessage { get; set; }
        public TimeSpan Duration { get; set; }
        public IReadOnlyDictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}
