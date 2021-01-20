using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiCore3Swagger.Middleware;
using WebApiCore3Swagger.Models.Auth;

namespace WebApiCore3Swagger.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.2")]
  //  [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [AllowAnonymous]
    public class ValuesController : ControllerBase
    {

        /// <summary>
        /// This end point require basic authentication to access the data
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("")]
        [MapToApiVersion("2.2")]
        public async Task<ActionResult<string>> Get()
        {
            var name = await Task.FromResult(User.Identity.Name);
            var requestQueryUrl = string.Concat( Request.Scheme ,"://" , Request.Host.ToUriComponent(), Request.PathBase.ToUriComponent(), Request.Path.ToUriComponent());
            var keyval = HttpContext.Items[HeaderKeyToken.HeaderTokenKey];
            

          return Ok($"[value, value2, you name is: {name}, endpointkey is : {keyval}]");
        }

        /// <summary>
        /// There are restriction on this route
        /// </summary>
        /// <param name="Lat">This value must be greater than zero</param>
        /// <param name="Long">This value must be lest than zero</param> 
        /// <returns></returns>
        /// <remarks>
        /// <font color="red">There are latitude and longitude restriction on locations</font> 
        /// </remarks>
        [HttpGet, Route("GetLatLong/{Lat:LatLongContraint}/{Long}")]
        [MapToApiVersion("2.2")]
        public async Task<ActionResult<string>> GetLatLongResults(double Lat, double Long)
        {
            var result = await Task.FromResult( $"Latitude {Lat} Longitude => {Long}");

            return result;
        }

        [HttpPost, Route("LoginInfo")]
        [MapToApiVersion("2.2")]
        public async Task<ActionResult<string>> PostedLoggedIn([FromBody] RequestAuthUser requestAuthUser)
        {
            var result = await Task.FromResult("Result Processed");


            return Ok(result);

        }

        [HttpGet, Route("VeiwExecutiveReport")]
        [MapToApiVersion("2.2")]
        [Produces(contentType: "application/xml", additionalContentTypes: new string[] { "application/json","text/csv" })]
        public async Task<IActionResult> GetViewData(bool getHeaders = false)
        {
            IList<HeaderView> list = null; // new List<HeaderView>();
           
            await Task.Run(() =>
            {
                list = new List<HeaderView>{
                new HeaderView { HeaderID = 1, ColumnName = "Column1" },
                new HeaderView { HeaderID = 2, ColumnName = "Column2" }
            };
            }).ConfigureAwait(false);
            
           if(getHeaders)
            {
                return Ok(list);
            }

            IList<ViewData> listData = null;
           await Task.Run(() =>   listData = new List<ViewData>
            {
               new ViewData { Name ="David Chen", Address= "60 Pavane LinkWay", PhoneNumber= "416-889-9876"},
               new ViewData { Name ="Shannon Walker", Address= "33 Old Mill Road", PhoneNumber= "416-990-9876"},

            });

           return Ok(listData);
        }
    }

   
    public class HeaderView
    {
        public int HeaderID { get; set; }
        public string ColumnName { get; set; }

    }

    public class ViewData
    {
        public string Name { get; set; }
        public string  Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}