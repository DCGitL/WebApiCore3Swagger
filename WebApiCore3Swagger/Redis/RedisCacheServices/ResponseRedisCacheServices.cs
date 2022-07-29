using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.RedisCacheServices
{
    //nuget pagage Microsoft.Extensions.Caching.StackExchangeRedis
    public class ResponseRedisCacheServices : IResponseCacheService
    {
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<ResponseRedisCacheServices> logger;

        public ResponseRedisCacheServices(IDistributedCache distributedCache, ILogger<ResponseRedisCacheServices> logger)
        {
            this.distributedCache = distributedCache;
            this.logger = logger;
        }
        public async Task CachedResponseAsync(string cacheKey, object response, TimeSpan timeALive)
        {
            if( response == null)
            {
                return;
            }

            var serializedResponse = JsonConvert.SerializeObject(response);

            var distributedCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeALive
            };
            await distributedCache.SetStringAsync(cacheKey, serializedResponse, distributedCacheOptions);
            
        }

        public async Task<string> GetCachedResponeAsync(string cacheKey)
        {
            string cacheResponse = String.Empty;

            try
            {
                cacheResponse = await distributedCache.GetStringAsync(cacheKey);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Redis Cache Logging Error", this);
            }

            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }
    }
}
