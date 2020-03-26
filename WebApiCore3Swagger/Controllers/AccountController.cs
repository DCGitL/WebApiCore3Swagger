using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApiCore3Swagger.Models.Account;
using WebApiCore3Swagger.Models.IdentityDbContext;

namespace WebApiCore3Swagger.Controllers
{
    /// <summary>
    /// This controller usess jwt token authentication
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]    
    [ApiController]
    [ApiVersion("2.2")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
     public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>
        /// Get a list of Api Users must be authenticated by jwt token to access this endpoint
        /// </summary>
        /// <remarks>
        /// This end point has an authorization policy attached to it => Mustbedacid.comandAdmin
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("2.2")]
        [Route("GetAccountUsers")]
        [Authorize(Policy= "MustBedavidcomandAdmin")]
        public async Task<ActionResult<IEnumerable<ResponseUser>>> GetAccountUsers()
        {

            var users = userManager.Users.Select(u => new ResponseUser
            {
                City = u.City,
                UserName = u.UserName,
                Email = u.Email,
               
            }).ToList() ;

            if(users == null || users.Count == 0)
            {
               return await Task.FromResult( NotFound());
            }

            foreach(var user in users)
            {
                var appUser = await userManager.FindByNameAsync(user.UserName);
                var roles = await userManager.GetRolesAsync(appUser);
                if(roles != null && roles.Count > 0)
                {
                    user.Roles = roles.ToList();
                }
               
            }

            return Ok(users);
        }

    }
}