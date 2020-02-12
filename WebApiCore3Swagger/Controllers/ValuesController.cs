using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApiCore3Swagger.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.2")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class ValuesController : ControllerBase
    {

        /// <summary>
        /// This end point require basic authentication to access the data
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        [MapToApiVersion("2.2")]
        public ActionResult<string> Get()
        {
            var name = User.Identity.Name;

          return  $"[value, value2, you name is: {name}]";
        }
    }
}