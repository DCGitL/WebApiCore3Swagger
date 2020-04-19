using Adventure.Works._2012.dbContext.Northwind.Repository;
using Adventure.Works._2012.dbContext.ResponseModels;
using MessageManager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebApiCore3Swagger.Infrastructor;
using WebApiCore3Swagger.RedisCache.Attribs.Settings;

namespace WebApiCore3Swagger.Controllers.Northwind
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("3.1")]
    // [Authorize(AuthenticationSchemes = "BasicAuthentication")]
  //  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [Authorize(Roles = "Admin")]
    public class EmployeeController : ControllerBase
    {
        private readonly INorthwindRepository northwindRepository;
        private readonly ISendEmail sendmail;
        private readonly IWebHostEnvironment environment;

        public EmployeeController(INorthwindRepository northwindRepository, ISendEmail sendmail, IWebHostEnvironment environment)
        {
            this.northwindRepository = northwindRepository;
            this.sendmail = sendmail;
            this.environment = environment;
        }

        /// <summary>
        /// Returns a list of Northwind employees in json or xml format
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// <font color="red">This end point uses basic authentication an redis cache and has a policy for authorization to access this end point note data is cached for 60 seconds</font>
        /// </remarks>
        [HttpGet, Route("GetEmployees")]
        [MapToApiVersion("3.1")]
        [Produces(contentType: "application/json", additionalContentTypes: new string[] { "application/xml" })]
        [RedisCached(60)]
        //  [Authorize(Policy = "MustBedavidcomandAdmin")]
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
        [RedisCached(60)]
        public async Task<ActionResult<IEnumerable<ResponseEmployee>>> GetEmployee(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException($"{id} is not a valid id");
            }

            var results = await northwindRepository.GetAsyncEmployee(id);
            string val = JsonConvert.SerializeObject(results);
            await sendmail.SendMail(val, EnumEmailType.plaintext);

            return Ok(results);
        }


        [HttpGet, Route("GetNorthwindOrders")]
        [MapToApiVersion("3.1")]
        public async Task<ActionResult<IEnumerable<ResponseOrder>>> GetAllOrders()
        {
            var orders = await northwindRepository.GetAllOrders();

            if (orders == null)
            {
                return NotFound("No order found");
            }

            return Ok(orders);
        }

        [HttpGet, Route("GetEmployeesJsXmlCsv")]
        [MapToApiVersion("3.1")]
        [Produces("application/json", additionalContentTypes: new string[] { "application/xml","text/xml","text/csv" },Type =typeof(List<object>))]
        public async Task<IActionResult> GetAllJsonStringEmployees()
        {
            var accepttype = Request.Headers["Accept"];
            string employeesstr = string.Empty;
            if (accepttype == "text/csv")
            {
                var csv = await northwindRepository.GetAllCSVStringEmployeesAsync();


                byte[] filebytes = new byte[csv.Length * sizeof(char)];
                System.Buffer.BlockCopy(csv.ToCharArray(), 0, filebytes, 0, filebytes.Length);


                return File(filebytes, "text/csv", "employee.csv");
            }
            else if (accepttype == @"text/xml" || accepttype == @"application/xml")
            {
                employeesstr = await northwindRepository.GetAllXmlStringEmployeesAsync();

                if (string.IsNullOrEmpty(employeesstr))
                {
                    return NotFound("No order found");
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(employeesstr);

                XmlElement root = xml.DocumentElement;

                return Ok(root).ForceResultAsXml();
            }
            else
            {
                employeesstr = await northwindRepository.GetAllJsonStringEmployeesAsync();

                var jsstring = "{\"employees\":" + employeesstr + "}";
                var jobject = JObject.Parse(jsstring);
                return Ok(jobject);

            }


        }

        [HttpGet, Route("GetJsonString")]
        [MapToApiVersion("3.1")]
        [Produces("application/json")]
        public async Task<IActionResult> GetJsonData()
        {

          var val = await   Task.Run(() =>
            {
                string json = string.Empty;
                var folderDetails = environment.ContentRootPath + environment.WebRootPath;
                var filePath = $"{folderDetails}\\JsonData\\response_1585576005933.json";
                 json = System.IO.File.ReadAllText(filePath);
                return json;
            });
          var jsonObj = JObject.Parse(val);
           
            return Ok(jsonObj);

        }
    }
}