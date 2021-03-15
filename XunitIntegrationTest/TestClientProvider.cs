using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net.Http;
using WebApiCore3Swagger;

namespace XunitIntegrationTest
{
    public class TestClientProvider
    {

        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {
            //var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //if(root.IndexOf("bin") != -1)
            //{
            //    root = root.Substring(0, Assembly.GetExecutingAssembly().Location.IndexOf("bin"));
            //}
            var contentRoot = Directory.GetCurrentDirectory();
            var configurationBuilder = new ConfigurationBuilder()
               .SetBasePath(contentRoot)
               .AddJsonFile("appsettings.json");

            var builder = new WebHostBuilder()
                .UseConfiguration(configurationBuilder.Build())
                .UseEnvironment("Development")
                .UseStartup<Startup>() ;
                
            var server = new TestServer(builder);
           
            Client = server.CreateClient();
        }
    }
}
