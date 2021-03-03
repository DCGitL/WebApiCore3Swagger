using EmployeeDB.Dal.Employee.DbRepository;
using EmployeeDB.Dal.EmployeeDbResponseModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Fluent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCore3Swagger.GenericsHelper;
using WebApiCore3Swagger.NLogger;
using WebApiCore3Swagger.RedisCache.Attribs.Settings;

namespace WebApiCore3Swagger.Controllers.EmployeeDb
{
    /// <summary>
    /// Jwt authentication is required to access this controller
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.1")]
    public class EmployeeDbController : ControllerBase
    {
        private readonly IEmployeeDbRepository employeeDbRepository;
        private readonly ILogger<EmployeeDbController> logger;
        private readonly ICustomNlogProperties loggerProperty;

        public EmployeeDbController(IEmployeeDbRepository employeeDbRepository, ILogger<EmployeeDbController> logger, ICustomNlogProperties loggerProperty)
        {
            this.employeeDbRepository = employeeDbRepository;
            this.logger = logger;
            this.loggerProperty = loggerProperty;
        }

    
        /// <summary>
        /// Jwt authentication required to access this end point
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllDbEmployees")]
        [MapToApiVersion("3.1")]
        [RedisCached(60)]
        public async Task<ActionResult<IEnumerable<EmployeeDbResponse>>> GetallDbEmployees()
        {
            var user = User.Identity.Name;
            var results = await employeeDbRepository.GetEmployeeDbsAsync();
            MyGenericEnumerable<EmployeeDbResponse> myGeneric = new MyGenericEnumerable<EmployeeDbResponse>();
            var csvstr = myGeneric.GetDelimitedString(results, ',');
            if(results == null || results.Count<EmployeeDbResponse>() == 0 )
            {
                return NotFound();
            }
        
            loggerProperty.LogProperty(new CustomProperty
            {
                Level = NLog.LogLevel.Info,
                UserName = user,
                LoggerName = $"{nameof(EmployeeDbController)}/{ nameof(GetallDbEmployees)}",
                Message = $"{results.Count<EmployeeDbResponse>()} employees requested"
            });
            return Ok(results);
        }

        /// <summary>
        /// Jwt authentication is required to access this end point
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, Route("GetDbEmployee/{id:int}")]
        [MapToApiVersion("3.1")]
        public async Task<ActionResult<EmployeeDbResponse>> GetDbEmployee(int id)
        {
            var result = await employeeDbRepository.GetEmployeeDbAsync(id);
            if(result == null)
            {
                return NotFound($"Employee with id {id} does not exist");
            }
            return Ok(result);
        }

        /// <summary>
        /// Jwt authentication is required to access this end point
        /// </summary>
        /// <param name="responseEmployee"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateEmployee")]
        [MapToApiVersion("3.1")]
        public async Task<ActionResult<EmployeeDbResponse>> CreateEmployee(EmployeeDbResponse responseEmployee)
        {
            var result = await employeeDbRepository.CreateEmployeeDbAsync(responseEmployee);

            var url = string.Concat(Request.Scheme, "://", Request.Host.ToUriComponent(), Request.PathBase.ToUriComponent(), Request.Path.ToUriComponent());
            var uri = url.Replace(nameof(CreateEmployee), nameof(GetDbEmployee));
            var val = Created($"{uri}/{result.Id}", result);
           

            return val;
        }

        /// <summary>
        /// Jwt authentication is required to access this end point
        /// </summary>
        /// <param name="responseEmployee"></param>
        /// <returns></returns>
        [HttpPut, Route("UpdateDbEmployee")]
        [MapToApiVersion("3.1")]
        public async Task<ActionResult<EmployeeDbResponse>> UpdateDbEmployee(EmployeeDbResponse responseEmployee)
        {
            var result = await employeeDbRepository.UpdateEmployeDbAsync(responseEmployee);

            if(result == null)
            {
                return NotFound("This employee could not be updated because it was not found");
            }

            return Ok(result);
        }

        /// <summary>
        /// Jwt authentication is required to access this end point
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteDbEmployee/{Id:int}")]
        [MapToApiVersion("3.1")]
        public async Task<ActionResult<EmployeeDbResponse>> DeleteDbEmployee(int Id)
        {
             await employeeDbRepository.DeleteEmployeeDbAsync(Id);

            return Ok("Record successfully deleted");
            ;
        }



    }
}