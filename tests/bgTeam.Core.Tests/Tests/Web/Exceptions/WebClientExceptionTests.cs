using bgTeam.Web.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Core
{
    public class QueueWarningExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            var ex = await Assert.ThrowsAsync<WebClientException>(() => throw new WebClientException(HttpStatusCode.Forbidden));
            Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
        }

        [Fact]
        public async Task ExceptionWithMessage()
        {
            var ex = await Assert.ThrowsAsync<WebClientException>(() => throw new WebClientException("msg", HttpStatusCode.Forbidden));
            Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public async Task ExceptionWithInnerException()
        {
            var ex = await Assert.ThrowsAsync<WebClientException>(() => throw new WebClientException("msg", new Exception(), HttpStatusCode.Forbidden));
            Assert.Equal(HttpStatusCode.Forbidden, ex.StatusCode);
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
    }
}
