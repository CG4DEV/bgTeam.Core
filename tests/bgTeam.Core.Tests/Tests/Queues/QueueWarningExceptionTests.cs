using bgTeam.Web.Exceptions;
using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues
{
    public class QueueWarningExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync<QueueWarningException>(() => throw new QueueWarningException());
        }

        [Fact]
        public async Task ExceptionWithMessage()
        {
            var ex = await Assert.ThrowsAsync<QueueWarningException>(() => throw new QueueWarningException("msg"));
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public async Task ExceptionWithInnerException()
        {
            var ex = await Assert.ThrowsAsync<QueueWarningException>(() => throw new QueueWarningException("msg", new Exception()));
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
        }
    }
}
