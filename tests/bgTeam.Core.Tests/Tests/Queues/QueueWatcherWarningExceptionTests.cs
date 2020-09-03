using bgTeam.Queues;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues
{
    public class QueueWatcherWarningExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            await Assert.ThrowsAsync<QueueWatcherWarningException>(() => throw new QueueWatcherWarningException());
        }

        [Fact]
        public async Task ExceptionWithMessage()
        {
            var ex = await Assert.ThrowsAsync<QueueWatcherWarningException>(() => throw new QueueWatcherWarningException("msg"));
            Assert.Equal("msg", ex.Message);
        }

        [Fact]
        public async Task ExceptionWithInnerException()
        {
            var ex = await Assert.ThrowsAsync<QueueWatcherWarningException>(() => throw new QueueWatcherWarningException("msg", new Exception()));
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
        }

        [Fact]
        public void SerializingException()
        {
            Assert.NotNull(JsonConvert.DeserializeObject<QueueWatcherWarningException>(JsonConvert.SerializeObject(new QueueWatcherWarningException())));
        }
    }
}
