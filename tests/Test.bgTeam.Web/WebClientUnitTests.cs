using bgTeam;
using bgTeam.Impl;
using bgTeam.Web.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Formatters.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Test.bgTeam.Web
{
    public sealed class WebClientUnitTests : IDisposable
    {
        private bool _disposed = false;
        private string _hostUrl;
        private IWebHost _host;
        private Task _hostTask;
        private IAppLogger _appLogger;
        private CancellationTokenSource _token;

        public WebClientUnitTests()
        {
            _hostUrl = "http://localhost:19876";
            _appLogger = new AppLoggerDefault();
            _token = new CancellationTokenSource();

            _host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(s =>
                {
                    s.AddMvcCore()
                    .AddFormatterMappings()
                    .AddJsonOptions(opt => opt.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented)
                    .AddJsonFormatters();
                })
                .Configure(app =>
                {
                    app.UseMvc();
                })
                .UseUrls(_hostUrl)
                .Build();

            _hostTask = _host.RunAsync(_token.Token);
        }

        [Fact]
        public async Task Get_String()
        {
            var webClient = new WebClient(_appLogger, _hostUrl);
            var result = await webClient.GetAsync<string>("Test/GetString").ConfigureAwait(false);

            Assert.Equal("GetString", result);
        }

        [Fact]
        public async Task Post_String()
        {
            var webClient = new WebClient(_appLogger, _hostUrl);
            var result = await webClient.PostAsync<string>("Test/PostString").ConfigureAwait(false);

            Assert.Equal("PostString", result);
        }

        [Fact]
        public async Task Post_NullString()
        {
            var webClient = new WebClient(_appLogger, _hostUrl);
            var result = await webClient.PostAsync<string>("Test/PostNullString").ConfigureAwait(false);

            Assert.Null(result);
        }

        [Fact]
        public async Task Post_ArrayString()
        {
            var webClient = new WebClient(_appLogger, _hostUrl);
            var result = await webClient.PostAsync<string[]>("Test/PostArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Post_EmptyArrayString()
        {
            var webClient = new WebClient(_appLogger, _hostUrl);
            var result = await webClient.PostAsync<string[]>("Test/PostEmptyArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _token?.Cancel();
                _host?.Dispose();
            }

            _disposed = true;
        }
    }
}
