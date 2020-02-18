using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApiCore3Swagger.SwaggerFilters
{
    public class ReplaceVersionWithValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
           IDictionary<string, OpenApiPathItem> pathItems = swaggerDoc.Paths
                .ToDictionary(p => p.Key.Replace("v{version}", swaggerDoc.Info.Version), p => p.Value);
            swaggerDoc.Paths.Clear();
            foreach(var pathItem in pathItems)
            {
                swaggerDoc.Paths.Add(pathItem.Key, pathItem.Value);
            }
        }
    }
}
