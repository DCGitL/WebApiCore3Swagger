﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.RedisCache.Attribs.Settings
{
    public class RedisCacheSettings
    {
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
    }
}
