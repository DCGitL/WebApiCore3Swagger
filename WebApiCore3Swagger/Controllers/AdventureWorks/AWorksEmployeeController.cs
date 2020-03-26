using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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


    /// <summary>
    /// Bearer Authentication is required to acces these endpoints
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.1")]
  // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        /// <remarks>
        /// <font color="red">Bearer Jwt token is required to access this end point</font>
        /// </remarks>
        [HttpGet, Route("GetAdventureWorksEmployees")]
        [MapToApiVersion("3.1")]
      //  [RedisCached(600)]
        public async Task<ActionResult<IEnumerable<ResponseAwEmployee>>> GetAwEmployees()
        {
            var results = await aworkrepository.GetAdventureWorksEmployeesAsync();
            if(results == null )
            {
                return NotFound("No employees were found");
            }

            return Ok(results);
        }


        [HttpGet, Route("GetAdventureWorksEmployee")]
        [MapToApiVersion("3.1")]
       // [RedisCached(60)]
        public async Task<ActionResult<ResponseAwEmployee>> GetEmployees(int id)
        {
            var results = await aworkrepository.GetEmployee(id);
            if (results == null)
            {
                return NotFound("No employees were found");
            }

            return Ok(results);
        }

        [HttpGet, Route("GetAdventureWorksEmployeesPhoto")]
        [MapToApiVersion("3.1")]
        // [RedisCachedAttribute(60)]
        public async Task<IActionResult> GetEmployeePhoto(int employeeId)
        {

           var memorystream = await aworkrepository.GetEmployeePhoto(employeeId);
            byte[] bytesarray = new byte[memorystream.Capacity];
            memorystream.Read(bytesarray, 0, memorystream.Capacity);

          return   File(bytesarray, "application/octet-stream","photo.png");

         
        }

    }
}