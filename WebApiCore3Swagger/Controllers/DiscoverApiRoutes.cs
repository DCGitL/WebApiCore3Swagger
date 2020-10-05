using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace WebApiCore3Swagger.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.2")]
    [AllowAnonymous]
    public class DiscoverApiRoutes : ControllerBase
    {
        private readonly IActionDescriptorCollectionProvider actionDescriptor;
        private readonly EndpointDataSource endpointData;

        public DiscoverApiRoutes(IActionDescriptorCollectionProvider actionDescriptor, EndpointDataSource endpointData)
        {
            this.actionDescriptor = actionDescriptor;
            this.endpointData = endpointData;
        }


        [MapToApiVersion("2.2")]
        [HttpGet]
        [Route("GetAllApiRoutes")]
        public ActionResult<IEnumerable<EndpointInfo>> GetAllApiRoutes()
        {
            List<EndpointInfo> routes = new List<EndpointInfo>();
            var items = actionDescriptor.ActionDescriptors.Items;
            var endpointDatas = endpointData.Endpoints;
            foreach(var item in items)
            {
                            
                var id = item.Id;
                var routeAcctionName = item.RouteValues["Action"];
                var routeControllerName = item.RouteValues["Controller"];
                var apiparamscount = item.Parameters.Count;
                var properties = item.Properties;
                var endpointverb = item.ActionConstraints?.OfType<HttpMethodActionConstraint>()?.FirstOrDefault()?.HttpMethods.FirstOrDefault();
                var versionmodel = item.GetApiVersionModel();
                var versions = versionmodel.DeclaredApiVersions; // ImplementedApiVersions;
                if(versions != null && versions.Count > 0)
                {
                    foreach(var ver in versions)
                    {
                        var majorv = ver.MajorVersion;
                        var minorv = ver.MinorVersion;
                        var v = $"{majorv}.{minorv}";
                        var implementedversion = versionmodel?.ImplementedApiVersions;

                        var link = $"{item?.AttributeRouteInfo?.Template}";
                        link = link.Replace("{version:apiVersion}", v);
                        routes.Add(new EndpointInfo
                        {
                            Name = $"{routeControllerName} {routeAcctionName}",
                            endpoint = link,
                            Controller = routeControllerName,
                            verb = endpointverb,
                            Parametercount = apiparamscount
                            
                        });

                    }

                }
               


                // var template = item.AttributeRouteInfo.Template;
            }
            
            return Ok(routes);

        }


        public class EndpointInfo
        {
            public string verb { get; set; }

            public string  Controller { get; set; }
            public string  endpoint { get; set; }

            public string Name { get; set; }

            public int Parametercount { get; set; }
        }
    }
}
