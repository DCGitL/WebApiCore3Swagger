using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Authorizations
{
    public class CustomizedAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Role { get; }
        public string domainName { get; }
        public CustomizedAuthorizationRequirement()
        {
            Role = "Admin";
            domainName = "david.com";
        }
    }
}
