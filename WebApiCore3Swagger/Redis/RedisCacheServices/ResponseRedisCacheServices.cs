using Adventure.Works._2012.dbContext.ResponseModels;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProtoBuf;
using System;
using System.IO;
using System.Threading.Tasks;
using TestLib.ResponseModels;

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
            string serializedResponse = string.Empty;
            if (response == null)
            {
                return;
            }

            var distributedCacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeALive
            };

            if (response is ResponseEmployee)
            {
               var protobufEmployee = CovertResponseEmployeeToProtobofferEmployee((ResponseEmployee)response);

                var protobufByteArry = ProtoSerialize(protobufEmployee);
                await distributedCache.SetAsync(cacheKey, protobufByteArry, distributedCacheOptions);
                return;
            }
                

             serializedResponse = JsonConvert.SerializeObject(response);

           
            await distributedCache.SetStringAsync(cacheKey, serializedResponse, distributedCacheOptions);

        }

        private byte[] ProtoSerialize<T>(T record) where T : class
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, record);
            return stream.ToArray();
        }

        private ResponseEmployeeProtobuf CovertResponseEmployeeToProtobofferEmployee(ResponseEmployee responseEmployee)
        {
            return new ResponseEmployeeProtobuf
            {
                EmployeeId = responseEmployee.EmployeeId,
                Address = responseEmployee.Address,
                City = responseEmployee.City,
                Region = responseEmployee.Region,
                Country = responseEmployee.Country,
                FirstName = responseEmployee.FirstName,
                LastName = responseEmployee.LastName,
                HomePhone = responseEmployee.HomePhone,
                PostalCode = responseEmployee.PostalCode,
                Title = responseEmployee.Title,
            };
        }
        public async Task<string> GetCachedResponeAsync(string cacheKey)
        {
            string cacheResponse = String.Empty;

            try
            {
                cacheResponse = await distributedCache .GetStringAsync(cacheKey);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Redis Cache Logging Error {THIS}", this);
            }

            return string.IsNullOrEmpty(cacheResponse) ? null : cacheResponse;
        }
    }
}
