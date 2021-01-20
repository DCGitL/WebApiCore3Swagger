using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiCore3Swagger.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("3.1")]
   // [Authorize]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMemoryCache cache;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMemoryCache cache)
        {
            _logger = logger;
            this.cache = cache;
        }

        [HttpGet]
        [MapToApiVersion("3.1")]
        [Route("GetWeatherForeCast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            object cacheKey = "WeatherKey";

            IEnumerable<WeatherForecast> returnvalue;
            if(!cache.TryGetValue<IEnumerable<WeatherForecast>>(cacheKey, out returnvalue))
            {
                // Key not in cache, so get data
                returnvalue =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
            .ToArray();

                // set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                   // keep in cache for this time, reset time if accessed.
                   .SetSlidingExpiration(TimeSpan.FromSeconds(10));

                //save data in cache
                cache.Set("WeatherKey", returnvalue, cacheEntryOptions);

            }
            _logger.LogInformation("Weather information from weather station");
            return returnvalue;
        }

        [HttpGet]
        [MapToApiVersion("3.1")]
        [Route("GetCurrentEndpoint")]
        public string  GetCurrentEndpoint()
        {
            string url = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent(), Request.PathBase.ToUriComponent(),Request.Path.ToUriComponent());

            return url;
        }
    }
}
