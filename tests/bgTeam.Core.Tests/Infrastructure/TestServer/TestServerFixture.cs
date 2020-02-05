using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;

namespace bgTeam.Core.Tests.Infrastructure.TestServer
{
    public class TestServerFixture
    {
        public IHost Host { get; }
        public string BaseUri { get; } = "http://localhost:19876";

        public TestServerFixture()
        {
            var hostBuilder = new HostBuilder()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseKestrel()
                        .UseStartup<Startup>()
                        .UseUrls(BaseUri);
                });
            Host = hostBuilder.Start();
        }

        ~TestServerFixture()
        {
            Host.StopAsync();
        }

    }
}
