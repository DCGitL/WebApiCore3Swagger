using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestLib.AdventureWorks.Repository;
using TestLib.ResponseModels;
using WebApiCore3Swagger.RedisCache.Attribs.Settings;

namespace WebApiCore3Swagger.Controllers.AdventureWorks
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.1")]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AWorksEmployeeController : ControllerBase
    {
        private readonly IAdventureWorksRepository aworkrepository;

        public AWorksEmployeeController(IAdventureWorksRepository aworkrepository)
        {
            this.aworkrepository = aworkrepository;
        }


        /// <summary>
        /// Get Adventure works employees
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAdventureWorksEmployees")]
        [MapToApiVersion("3.1")]
        [RedisCachedAttribute(600)]
        public async Task<ActionResult<IEnumerable<ResponseAwEmployee>>> GetAwEmployees()
        {
            var results = await aworkrepository.GetAdventureWorksEmployeesAsync();
            if(results == null )
            {
                return NotFound("No employees were found");
            }

            return Ok(results);
        }
    }
}