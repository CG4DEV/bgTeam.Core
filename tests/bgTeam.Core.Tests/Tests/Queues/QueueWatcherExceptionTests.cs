using bgTeam.Queues;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues
{
    public class QueueWatcherExceptionTests
    {
        [Fact]
        public async Task Exception()
        {
            var queueMessage = new Mock<IQueueMessage>();
            queueMessage.Setup(x => x.Body)
                .Returns("hi");
            var ex = await Assert.ThrowsAsync<QueueWatcherException>(() => throw new QueueWatcherException("msg", queueMessage.Object, new Exception()));
            Assert.Equal("msg", ex.Message);
            Assert.NotNull(ex.InnerException);
            Assert.NotNull(ex.QueueMessage);
            Assert.Equal("hi", ex.QueueMessage.Body);
        }
    }
}
