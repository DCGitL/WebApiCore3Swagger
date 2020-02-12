using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.SwaggerFilters
{

    /// <summary>
    /// This does not work when using openapiOperation swagger will no add parameters to the header by this method
    /// </summary>
    public class AuthenticationHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterDescriptors = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            bool isAuthorized = filterDescriptors.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is AuthorizeFilter);

            bool isAllowAnonymous = filterDescriptors.Select(filterInfo => filterInfo.Filter)
                .Any(filter => filter is IAllowAnonymousFilter);

            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            if(operation.Tags[0].Name == "Values")
            if (context.ApiDescription.RelativePath.Contains("api/v{version}/Values")) // == "v2.2/api/Values")
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Description = "Jwt Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Title = "Bearer",
                        Type = "String",
                        Default = new OpenApiString("Bearer")
                    }

                });
                operation.Parameters.Add(new OpenApiParameter
                {
                    Description = "ApiKey",
                    Name = "Parameter",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Title = "Parameter",
                        Type = "String",
                        Default = new OpenApiString("apikeyValue")
                    }

                });
            }




            if(isAuthorized && !isAllowAnonymous)
            {
               if(operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }

                operation.Parameters.Add(new OpenApiParameter
                {
                    Description = "Jwt Authorization",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Title = "Bearer",
                        Type = "String",
                        Default = new OpenApiString("Bearer")
                    }

                });
            }
        }
    }
}
