using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Services.Auth
{
    public class AppSetting
    {
        public string JwtTokenSecret { get; set; }
        public TimeSpan TokenLifeTime { get; set; }
    }
}
