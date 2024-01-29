using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebApiCore3Swagger.Models.HealthCheck;
using Xunit;

namespace XunitIntegrationTest
{
    public class HealthCheckIntegrationTest
    {
        private readonly HttpClient _httpClient;
        public HealthCheckIntegrationTest()
        {
            _httpClient = new TestClientProvider().Client;
        }

        [Fact]
        public async Task GetHealthStatusResponseFromApi()
        {


            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);

            var response = await _httpClient.GetAsync("api/v2.2/Health/GetApiHealth");


            response.EnsureSuccessStatusCode();

            var responsecontent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responsecontent );
            HealthCheckResponse healthResponse =
                JsonConvert.DeserializeObject<HealthCheckResponse>(responsecontent);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
           
            var checkscount = healthResponse.Checks.Count<HealthCheck>() > 0;
            Assert.True(checkscount);
        }
    }
}
