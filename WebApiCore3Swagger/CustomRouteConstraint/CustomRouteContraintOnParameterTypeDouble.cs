using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.CustomRouteConstraint
{
    /// <summary>
    /// This validates the incoming routing parameters of type double to determine if these values 
    /// meet the requirements
    /// </summary>
    /// <remarks>
    /// This route contraint must be registered with services.Configure.RouteOptions and add to the options.ConstraintMap.Add see the configure services method for details
    /// </remarks>
    public class CustomRouteContraintOnParameterTypeDouble : IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            bool _lat = false;
            bool _long = false;
            double Lat;
            if(double.TryParse(values["Lat"].ToString(), out Lat))
            {
                if(Lat > 0)
                {
                    _lat = true;
                }
            }
            double Long;

            if(double.TryParse(values["Long"].ToString(), out Long ))
            {
                if(Long < 0)
                {
                    _long = true;
                }
            }

            return _lat && _long;
        }
    }
}
