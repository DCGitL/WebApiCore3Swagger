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
        public string PublicKeyLocation { get; set; }

        public string PrivateKeyLocation { get; set; }
    }
}
