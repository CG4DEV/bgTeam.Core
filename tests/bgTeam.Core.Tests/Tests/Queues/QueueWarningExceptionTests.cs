using System;
using System.Threading.Tasks;
using bgTeam.Queues.Exceptions;
using Newtonsoft.Json;
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

        [Fact]
        public void SerializingException()
        {
            Assert.NotNull(JsonConvert.DeserializeObject<QueueWarningException>(JsonConvert.SerializeObject(new QueueWarningException())));
        }
    }
}
