using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApiCore3Swagger.Controllers.Error
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [ApiVersion("2.2")]
    [ApiVersion("3.1")]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("/error")]   
        [MapToApiVersion("2.2")]
        public ActionResult<ProblemDetails> Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError(context.Error.StackTrace, $"Unhandled Exception occured at: {DateTime.Now}");
            return Problem();
        
        
        }

        [Route("/error-local-development")]    
        [MapToApiVersion("2.2")]
        public ActionResult<ProblemDetails> ErrorLocalDevelopment(
                [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

            logger.LogError(context.Error.StackTrace, $"Unhandled Exception occured at: {DateTime.Now}");

            return Problem(
                detail: context.Error.StackTrace,
                title: context.Error.Message);
        }

    }
}