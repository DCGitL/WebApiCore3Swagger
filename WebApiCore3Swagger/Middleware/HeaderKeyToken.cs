using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace WebApiCore3Swagger.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HeaderKeyToken
    {

        public static readonly object HeaderTokenKey = new object();
        private readonly RequestDelegate _next;

        public HeaderKeyToken(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            IHeaderDictionary headers = httpContext.Request.Headers;
            if(headers.TryGetValue("EndPointKey", out StringValues values))
            {
                if(StringValues.IsNullOrEmpty(values))
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return;

                }
               
                httpContext.Items[HeaderTokenKey] = values[0].ToString();
            }


           

             await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HeaderKeyTokenExtensions
    {
        public static IApplicationBuilder UseHeaderKeyToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HeaderKeyToken>();
        }
    }
}
