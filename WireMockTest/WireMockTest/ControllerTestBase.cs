using System;
using System.IO;
using WireMock.Server;
using WireMock.Settings;

namespace WireMockTest
{
    public class ControllerTestBase : IDisposable
    {
        protected WireMockServer Server { get; set; }

        protected ControllerTestBase()
        {
            var settings = new WireMockServerSettings
            {
                Urls = new[] { "http://localhost:9095" }
            };

            Server = WireMockServer.Start(settings);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "__admin", "mappings");
            Server.ReadStaticMappings(path);
        }

        public void Dispose()
        {
            Server.Stop();
        }
    }
}