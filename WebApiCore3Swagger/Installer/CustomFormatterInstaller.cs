using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Linq;
using WebApiCore3Swagger.Formatter;

namespace WebApiCore3Swagger.Installer
{
    public class CustomFormatterInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {

            services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/csv" });
            });

            var csvFormatterOptions = new CsvFormatterOptions();
            csvFormatterOptions.CsvDelimiter = ",";
            services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new CsvOutputFormatter(csvFormatterOptions));
                options.FormatterMappings.SetMediaTypeMappingForFormat("csv", MediaTypeHeaderValue.Parse("text/csv"));
            }).AddNewtonsoftJson();
        }
    }
}
