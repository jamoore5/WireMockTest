using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WireMockTest
{
    public class UnitTest1 : ControllerTestBase
    {
        [Fact]
        public async Task Test2()
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            var uri = new Uri("http://localhost:9095/interactionapi/");
            var credentials = new CredentialCache
            {
                {
                    uri, "NTLM",
                    new NetworkCredential("username",
                        "password",
                        "domain")
                }
            };
            var handler = new HttpClientHandler {Credentials = credentials};
            var client = new HttpClient(handler);

            var response = SendRequestAsync(
                client,
                "http://localhost:9095/interactionapi/SystemService.svc/User/search",
                "{\"userSearchCriteria\":{\"IncludeInactive\":true,\"Id\":\"66\"},\"userReturnOptions\":{\"IncludeContact\":true,\"ContactReturnOptions\":{\"ElectronicAddressFlags\":\"PrimaryEmail\"}}}",
                HttpMethod.Post).Result;


            Assert.True(response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpClient client, string endpoint, string contentBody,
            HttpMethod method, Dictionary<string, string> headers = null)
        {
            var httpRequestMessage = BuildRequestMessage(endpoint, contentBody, method, headers);
            return await SendRequestAsync(client, httpRequestMessage);
        }

        private HttpRequestMessage BuildRequestMessage(string endpoint, string contentBody, HttpMethod action,
            Dictionary<string, string> headers)
        {
            var uri = new Uri(endpoint);

            var httpRequestMessage = new HttpRequestMessage
            {
                Method = action,
                RequestUri = uri,
                Content = BuildContentBody(contentBody)
            };

            return httpRequestMessage;
        }

        private static StringContent BuildContentBody(string contentBody)
        {
            return new StringContent(contentBody, Encoding.UTF8, "application/json");
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpClient client,
            HttpRequestMessage httpRequestMessage)
        {
            var responseMessage = await client.SendAsync(httpRequestMessage);

            return responseMessage;
        }
    }
}