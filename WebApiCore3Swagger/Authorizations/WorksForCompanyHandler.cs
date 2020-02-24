using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApiCore3Swagger.Authorizations
{
    public class WorksForCompanyHandler : AuthorizationHandler<WorksForCompanyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WorksForCompanyRequirement requirement)
        {
            //other logics could come here to restrict user to access the end point at which this handler
            // is going to be placed
            //Note the below email address is set in the jwttoken claim when creating the token to issue to the user.
            var userEmailAddress =  context.User?.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            if(userEmailAddress.EndsWith(requirement.DomainName))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            context.Fail();
            return Task.CompletedTask;
        }
    }
}
