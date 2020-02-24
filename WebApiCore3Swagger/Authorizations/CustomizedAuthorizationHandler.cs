using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Authorizations
{
    public class CustomizedAuthorizationHandler : AuthorizationHandler<CustomizedAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomizedAuthorizationRequirement requirement)
        {
           
            string userEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            string userName = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
            if(!string.IsNullOrEmpty(userEmail))
            {
                var isAdmin = context.User.IsInRole(requirement.Role);
                if(userEmail.EndsWith(requirement.domainName) && isAdmin)
                {
                    context.Succeed(requirement);
                    return Task.CompletedTask;
                }
            }
            context.Fail();
            return Task.CompletedTask;
        }
    }
}
