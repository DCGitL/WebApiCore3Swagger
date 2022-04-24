using MessageManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCore3Swagger.RedisCacheServices;

namespace WebApiCore3Swagger.RedisCache.Attribs.Settings
{
    [AttributeUsage(validOn:AttributeTargets.Class | AttributeTargets.Method)]
    public class RedisCachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int timeToLiveSeconds;

        public RedisCachedAttribute(int timeToLiveSeconds)
        {
            this.timeToLiveSeconds = timeToLiveSeconds;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cachSettings = context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            var emailService = context.HttpContext.RequestServices.GetRequiredService<ISendEmail>();
          

            if(!cachSettings.Enabled)
            {
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCachedResponeAsync(cacheKey);
            if(!string.IsNullOrEmpty(cacheResponse))
            {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
              //  await  emailService.SendMail(cacheResponse, EnumEmailType.plaintext);
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();

            if(executedContext.Result is ObjectResult okObjectResult)
            {
                await cacheService.CachedResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
                var contentResult = new ContentResult
                {
                    Content = JsonConvert.SerializeObject(okObjectResult.Value),
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            };

        }


        private static string GenerateCacheFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach(var q in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{q.Key}---{q.Value}");
            }
            return keyBuilder.ToString();

        }
    }
}
