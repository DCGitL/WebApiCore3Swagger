using AutoMapper.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XunitIntegrationTest
{
    public class ApiAuthenticationTokenIntegrationTest
    {
        private readonly HttpClient client;
        public ApiAuthenticationTokenIntegrationTest()
        {
            client = new TestClientProvider().Client;
        }

        [Fact]
        public async Task GetTokenForAuthenticationTest_ReturnJwtToken()
        {
            //Arrange
            var userInfo = new UserInfo { userName = "dac@david.com", password = "1qaz@WSX" };
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            string jsonStringIfyData = JsonConvert.SerializeObject(userInfo);

            var contentData = new StringContent(jsonStringIfyData, System.Text.Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("/api/v2.2/Auth/Login", contentData);

            //Assert
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responsecontent = await response.Content.ReadAsStringAsync();
            AuthResponse tokenresponse = JsonConvert.DeserializeObject<AuthResponse>(responsecontent);
            var token = tokenresponse.accessToken;
            var expdate = tokenresponse.expirationDateTime;
            var issuedDate = tokenresponse.dateIssued;
            var refreshtoken = tokenresponse.refreshToken;
            Assert.NotNull(token);
            Assert.NotNull(expdate);
            Assert.NotNull(issuedDate);
            Assert.NotNull(refreshtoken);

            var currentUtcDatetime = DateTime.UtcNow;
            Assert.True(currentUtcDatetime < expdate);


        }
    }

}
