using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;
using Xunit;

namespace WireMock.Test
{
    public class UnitTest1 : IDisposable
    {
        private WireMockServer _server;
        private readonly string _baseUrl;
        private HttpClient _client;

        public UnitTest1()
        {
            var port = new Random().Next(5000, 6000);
            _baseUrl = "http://localhost:" + port;

            _server = WireMockServer.Start(new WireMockServerSettings{Urls = new []{_baseUrl}});

            _client = new HttpClient();
            AddDefaultJsonHeaders();
        }

        [Fact]
        public async Task Test1()
        {
            _server.Given(Request.Create().WithPath("/interactionapi/SystemService.svc/User/search").UsingPost().WithBody(new JsonMatcher("{\"userSearchCriteria\":{\"IncludeInactive\":true,\"Id\":\"66\"},\"userReturnOptions\":{\"IncludeContact\":true,\"ContactReturnOptions\":{\"ElectronicAddressFlags\":\"PrimaryEmail\"}}}")))
                .RespondWith(Response.Create().WithStatusCode(200).WithBody(@"{""key"": ""value""}"));

            var httpRequestMessage = BuildRequestMessage($"{_baseUrl}/interactionapi/SystemService.svc/User/search", "{\"userSearchCriteria\":{\"IncludeInactive\":true,\"Id\":\"66\"},\"userReturnOptions\":{\"IncludeContact\":true,\"ContactReturnOptions\":{\"ElectronicAddressFlags\":\"PrimaryEmail\"}}}", HttpMethod.Post);
            var responseMessage = await _client.SendAsync(httpRequestMessage);

            Assert.True(responseMessage.IsSuccessStatusCode);
        }

        private HttpRequestMessage BuildRequestMessage(string endpoint, string contentBody, HttpMethod action)
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

        private void AddDefaultJsonHeaders()
        {
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }


        public void Dispose()
        {
            _server?.Stop();
            _server?.Dispose();
            _client?.Dispose();
        }
    }
}
