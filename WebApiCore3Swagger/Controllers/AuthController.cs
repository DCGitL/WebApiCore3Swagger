using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiCore3Swagger.Models.Auth;
using WebApiCore3Swagger.Models.RefreshToken.Request.Response;

namespace WebApiCore3Swagger.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.2")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthJwtService authJwtService;

        public AuthController(IAuthJwtService authJwtService)
        {
            this.authJwtService = authJwtService;
        }


        [HttpPost, Route("Login")]
        [MapToApiVersion("2.2")]
        public async Task<ActionResult<ResponseAuth>> Login([FromBody] RequestAuthUser user)
        {
            var responseResult = await authJwtService.AsyncAuthenticate(user.userName, user.Password);

            if(responseResult == null)
            {
                return NotFound(new { message = $"User with user name {user.userName} could not found" });
            }

            return Ok(responseResult);
        }

        /// <summary>
        /// Get a refreshed token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpPost, Route("RefreshToken")]
        [MapToApiVersion("2.2")]
        //[SwaggerResponse(200, description: "Get user Information", Type = typeof(User))]
        [Produces(contentType: "application/json", additionalContentTypes: new string[] { "application/xml" })]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest refreshToken)
        {

           
            var tokenRefresh = await authJwtService.RefreshTokenAsyc(refreshToken.Token, refreshToken.RefreshToken);
            if (tokenRefresh == null)
            {
                return BadRequest(new { message = "Unable to create refreshed token" });
            }

            return Ok(tokenRefresh);

        }

    }
}