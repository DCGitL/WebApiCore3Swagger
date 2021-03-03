using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.NLogger
{
    public class CustomProperty
    {
        /// <summary>
        /// Provides the property data for @UserName parameter in nlog.config
        /// </summary>
        public string UserName { get; set; }

        public string Message { get; set; }
        public string LoggerName { get; set; }

        public LogLevel Level { get; set; }
    }
}
