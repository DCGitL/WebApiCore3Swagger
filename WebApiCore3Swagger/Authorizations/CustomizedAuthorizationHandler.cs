using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.IdentityDbContext;

namespace WebApiCore3Swagger.Authorizations
{
    public class CustomizedAuthorizationHandler : AuthorizationHandler<CustomizedAuthorizationRequirement> , IAuthorizationHandler
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        /// <summary>
        /// Inject IConfiguration into the constructor to get configuration values from appsettings.json file 
        /// Note not all object can be injected here
        /// </summary>
        /// <param name="configuration">Used to access congiguration set in the appsettings.json file</param>
        /// <param name="userManager">Used to access api users</param>

        public CustomizedAuthorizationHandler(IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
        }
        protected override async  Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomizedAuthorizationRequirement requirement)
        {
            var mykey = configuration.GetSection("Mykey").Value;

            if(!context.User.HasClaim(c=> c.Type == ClaimTypes.Email && c.Type == ClaimTypes.Name))
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            string userEmail = userEmail = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            string userName = userName = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                      

            var appuser = await userManager.FindByNameAsync(userName);

              
            if(!string.IsNullOrEmpty(userEmail))
            {
                var isAdmin = context.User.IsInRole(requirement.Role);
                if(userEmail.EndsWith(requirement.domainName) && isAdmin)
                {
                    context.Succeed(requirement);
                     await Task.CompletedTask;
                    return;
                }
            }
            context.Fail();
            await Task.CompletedTask;
        }
    }
}
