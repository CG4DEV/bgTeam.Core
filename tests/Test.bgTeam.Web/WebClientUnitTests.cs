﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using bgTeam.Web;
using bgTeam.Web.Builders;
using bgTeam.Web.Impl;
using bgTeam.Web.IoC;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Test.bgTeam.Web.Services;
using Xunit;

namespace Test.bgTeam.Web
{
    public sealed class WebClientUnitTests : IDisposable
    {
        private bool _disposed = false;
        private string _hostUrl;
        private IWebHost _host;
        private Task _hostTask;
        private CancellationTokenSource _token;

        private IWebClient _webClient;
        private IWebClientClone _webClientClone;

        public WebClientUnitTests()
        {
            _hostUrl = "http://localhost:19876";
            _token = new CancellationTokenSource();

            _host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureServices(s =>
                {
                    s.AddMvcCore()
                        .AddMvcOptions(opt => opt.EnableEndpointRouting = false)
                        .AddFormatterMappings()
                        .AddJsonOptions(opt => opt.JsonSerializerOptions.WriteIndented = true);

                    s.AddWebClient<IWebClient, WebClient>(_hostUrl)
                     .AddWebClient<IWebClientClone, WebClientClone>(_hostUrl + "/Test");
                })
                .Configure(app =>
                {
                    app.UseMvc();
                })
                .UseUrls(_hostUrl)
                .Build();

            _webClient = _host.Services.GetRequiredService<IWebClient>();
            _webClientClone = _host.Services.GetRequiredService<IWebClientClone>();
            _hostTask = _host.RunAsync(_token.Token);
        }

        [Fact]
        public async Task Get_String()
        {
            var result = await _webClient.GetAsync<string>("Test/GetString").ConfigureAwait(false);

            Assert.Equal("GetString", result);
        }

        [Fact]
        public async Task Delete_String()
        {
            var result = await _webClient.DeleteAsync<string>("Test/DeleteString").ConfigureAwait(false);

            Assert.Equal("DeleteString", result);
        }

        [Fact]
        public async Task Post_String()
        {
            var result = await _webClient.PostAsync<string>("Test/PostString").ConfigureAwait(false);

            Assert.Equal("PostString", result);
        }

        [Fact]
        public async Task Put_String()
        {
            var result = await _webClient.PutAsync<string>("Test/PutString").ConfigureAwait(false);

            Assert.Equal("PutString", result);
        }

        [Fact]
        public async Task Post_NullString()
        {
            var result = await _webClient.PostAsync<string>("Test/PostNullString").ConfigureAwait(false);

            Assert.Null(result);
        }

        [Fact]
        public async Task Put_NullString()
        {
            var result = await _webClient.PutAsync<string>("Test/PutNullString").ConfigureAwait(false);

            Assert.Null(result);
        }

        [Fact]
        public async Task Post_ArrayString()
        {
            var result = await _webClient.PostAsync<string[]>("Test/PostArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Put_ArrayString()
        {
            var result = await _webClient.PutAsync<string[]>("Test/PutArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task Post_EmptyArrayString()
        {
            var result = await _webClient.PostAsync<string[]>("Test/PostEmptyArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Put_EmptyArrayString()
        {
            var result = await _webClient.PutAsync<string[]>("Test/PutEmptyArrayString").ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_QueryString_String()
        {
            var query = new Dictionary<string, object> { { "query", "stringQuery" } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryString", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(query["query"], result);
        }

        [Fact]
        public async Task Get_QueryString_Int()
        {
            var expected = 12345678;
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryInt", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_Double()
        {
            var expected = 12345.678D;
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryDouble", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_DateTime()
        {
            var expected = DateTime.Now;
            var query = new Dictionary<string, object> { { "query", expected } };
            
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryDateTime", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_DateTimeOffset()
        {
            var expected = DateTimeOffset.Now;
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryDateTimeOffset", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_TimeSpan()
        {
            var expected = TimeSpan.FromMilliseconds(31241241124);
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryTimeSpan", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString("G", CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_Float()
        {
            var expected = 123.45678F;
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryFloat", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_QueryString_Decimal()
        {
            var expected = 123.45678M;
            var query = new Dictionary<string, object> { { "query", expected } };
            _webClient.Culture = CultureInfo.InvariantCulture;
            var result = await _webClient.GetAsync<string>("Test/GetQueryDecimal", query).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        }

        [Fact]
        public async Task Get_SendHeader()
        {
            var expected = "headerValue";
            var headers = new Dictionary<string, object> { { "test_header", expected } };
            var result = await _webClient.GetAsync<string>("Test/GetReturnHeaderValue", headers: headers).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Post_SendHeader()
        {
            var expected = "headerValue";
            var headers = new Dictionary<string, object> { { "test_header", expected } };
            var result = await _webClient.PostAsync<string>("Test/PostReturnHeaderValue", headers: headers).ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task Get_String_WithOffset()
        {
            var result = await _webClientClone.GetAsync<string>("GetString").ConfigureAwait(false);

            Assert.Equal("GetString", result);
        }

        [Fact]
        public async Task Post_String_WithOffset()
        {
            var result = await _webClientClone.PostAsync<string>("PostString").ConfigureAwait(false);

            Assert.Equal("PostString", result);
        }

        [Fact]
        public async Task Put_String_WithOffset()
        {
            var result = await _webClientClone.PutAsync<string>("PutString").ConfigureAwait(false);

            Assert.Equal("PutString", result);
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
