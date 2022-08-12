using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.RedisCacheServices
{
    public interface IResponseCacheService
    {
        Task CachedResponseAsync(string cacheKey, object response, TimeSpan timeALive);
        Task<string> GetCachedResponeAsync(string cacheKey);

    }
}
