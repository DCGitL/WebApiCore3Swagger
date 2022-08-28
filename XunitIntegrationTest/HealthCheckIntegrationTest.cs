using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
            var response = await _httpClient.GetAsync("/api/v2.2/Health/GetApiHealth");

            //  response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
