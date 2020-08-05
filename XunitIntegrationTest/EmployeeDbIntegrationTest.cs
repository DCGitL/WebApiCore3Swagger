using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApiCore3Swagger;
using Xunit;

namespace XunitIntegrationTest
{



    public class EmployeeDbIntegrationTest
    {
        private readonly HttpClient client;
        public EmployeeDbIntegrationTest()
        {
           
             client = new TestClientProvider().Client;
        }
        [Fact]
        public async Task GetAllDbEmployeeWithoutTokenTest_ReturnUnauthorized()
        {
           
            var response = await client.GetAsync("/api/v3.1/EmployeeDb/GetAllDbEmployees");

          //  response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

      
    }
}
