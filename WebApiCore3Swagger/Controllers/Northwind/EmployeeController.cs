using Adventure.Works._2012.dbContext.Northwind.Repository;
using Adventure.Works._2012.dbContext.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore3Swagger.RedisCache.Attribs.Settings;

namespace WebApiCore3Swagger.Controllers.Northwind
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.1")]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly INorthwindRepository northwindRepository;

        public EmployeeController(INorthwindRepository northwindRepository )
        {
            this.northwindRepository = northwindRepository;
        }

        /// <summary>
        /// Returns a list of Northwind employees in json or xml format
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetEmployees")]
        [MapToApiVersion("3.1")]
        [Produces(contentType:"application/json", additionalContentTypes: new string[] {"application/xml" })]
        [RedisCachedAttribute(600)]
        public async Task<ActionResult<IEnumerable<ResponseEmployee>>> GetEmployees()
        {
            var results = await northwindRepository.GetAllAsyncEmployees();
            
            return Ok(results);
        }

        /// <summary>
        /// Returns an employee with the requested Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetEmployee/{id:int}")]
        [MapToApiVersion("3.1")]
        [Produces(contentType: "application/json", additionalContentTypes: new string[] { "application/xml" })]
        public async Task<ActionResult<IEnumerable<ResponseEmployee>>> GetEmployee(int id)
        {
            if(id == 0)
            {
                throw new ArgumentException($"{id} is not a valid id");
            }

            var results = await northwindRepository.GetAsyncEmployee(id);

            return Ok(results);
        }
    }
}