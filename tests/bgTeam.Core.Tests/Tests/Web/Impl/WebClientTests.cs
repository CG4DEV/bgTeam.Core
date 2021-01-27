using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using bgTeam.Core.Tests.Infrastructure.TestServer;
using bgTeam.Web;
using bgTeam.Web.Impl;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Web.Impl
{
    [Collection("TestServerCollection")]
    public class WebClientTests
    {
        private readonly TestServerFixture _fixture;
        public WebClientTests(TestServerFixture fixture)
        {
            _fixture = fixture;
        }

        //[Fact]
        //public void DependencyContentBuilder()
        //{
        //    var contentBuilder = GetMocks();
        //    Assert.Throws<ArgumentNullException>("builder", () =>
        //    {
        //        new WebClient("http://test.com", (IContentBuilder)null);
        //    });
        //}

        //[Fact]
        //public async Task GetString()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetString").ConfigureAwait(false);

        //    Assert.Equal("GetString", result);
        //}

        //[Fact]
        //public async Task Post_String()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var result = await webClient.PostAsync<string>("WebClientTest/PostString").ConfigureAwait(false);

        //    Assert.Equal("PostString", result);
        //}

        //[Fact]
        //public async Task Post_NullString()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var result = await webClient.PostAsync<string>("WebClientTest/PostNullString").ConfigureAwait(false);

        //    Assert.Null(result);
        //}

        //[Fact]
        //public async Task Post_ArrayString()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var result = await webClient.PostAsync<string[]>("WebClientTest/PostArrayString").ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.NotEmpty(result);
        //}

        //[Fact]
        //public async Task Post_EmptyArrayString()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var result = await webClient.PostAsync<string[]>("WebClientTest/PostEmptyArrayString").ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Empty(result);
        //}

        //[Fact]
        //public async Task Get_QueryString_String()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var query = new Dictionary<string, object> { { "query", "stringQuery" } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryString", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(query["query"], result);
        //}

        //[Fact]
        //public async Task Get_QueryString_Int()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = 12345678;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryInt", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_Double()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = 12345.678D;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryDouble", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_DateTime()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = DateTime.Now;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryDateTime", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_DateTimeOffset()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = DateTimeOffset.Now;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryDateTimeOffset", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_TimeSpan()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = TimeSpan.FromMilliseconds(31241241124);
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryTimeSpan", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString("G", CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_Float()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = 123.45678F;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryFloat", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_QueryString_Decimal()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = 123.45678M;
        //    var query = new Dictionary<string, object> { { "query", expected } };
        //    webClient.Culture = CultureInfo.InvariantCulture;
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetQueryDecimal", query).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected.ToString(CultureInfo.InvariantCulture), result);
        //}

        //[Fact]
        //public async Task Get_SendHeader()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = "headerValue";
        //    var headers = new Dictionary<string, object> { { "test_header", expected } };
        //    var result = await webClient.GetAsync<string>("WebClientTest/GetReturnHeaderValue", headers: headers).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected, result);
        //}

        //[Fact]
        //public async Task Post_SendHeader()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri);
        //    var expected = "headerValue";
        //    var headers = new Dictionary<string, object> { { "test_header", expected } };
        //    var result = await webClient.PostAsync<string>("WebClientTest/PostReturnHeaderValue", headers: headers).ConfigureAwait(false);

        //    Assert.NotNull(result);
        //    Assert.Equal(expected, result);
        //}

        //[Fact]
        //public async Task Get_String_WithOffset()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri + "/WebClientTest");
        //    var result = await webClient.GetAsync<string>("GetString").ConfigureAwait(false);

        //    Assert.Equal("GetString", result);
        //}

        //[Fact]
        //public async Task Post_String_WithOffset()
        //{
        //    var webClient = new WebClient(_fixture.BaseUri + "/WebClientTest");
        //    var result = await webClient.PostAsync<string>("PostString").ConfigureAwait(false);

        //    Assert.Equal("PostString", result);
        //}

        private Mock<IContentBuilder> GetMocks()
        {
            var contentBuilder = new Mock<IContentBuilder>();
            return contentBuilder;
        }
    }
}
