using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace XunitIntegrationTest
{
    public class TestClientProvider
    {

        public HttpClient Client { get; private set; }
        public TestClientProvider()
        {
            var webApplicationFactory = new WebApplicationFactory<Program>();
            Client = webApplicationFactory.CreateDefaultClient();
           
        }
    }
}
