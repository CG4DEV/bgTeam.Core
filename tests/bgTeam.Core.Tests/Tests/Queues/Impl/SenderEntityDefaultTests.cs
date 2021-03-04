using System;
using bgTeam.Queues;
using bgTeam.Queues.Exceptions;
using bgTeam.Queues.Impl;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues.Impl
{
    public class SenderEntityDefaultTests
    {
        [Fact]
        public void DependencyQueueProvider()
        {
            var queueProvider = GetMocks();
            Assert.Throws<ArgumentNullException>("queueProvider", () =>
            {
                new SenderEntityDefault(null);
            });
        }

        [Fact]
        public void SendEntity()
        {
            var queueProvider = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(queueProvider.Object);
            senderEntityDefault.Send(new QueueMessageDefault { Body = "Hi" }, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"));
        }

        [Fact]
        public void SendEntityWithExceptionShouldRetryAttemption5Times()
        {
            var queueProvider = GetMocks();
            queueProvider.Setup(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"))
                .Throws(new Exception());
            var senderEntityDefault = new SenderEntityDefault(queueProvider.Object);
            Assert.ThrowsAny<SenderEntityException>(() => senderEntityDefault.Send(new QueueMessageDefault { Body = "Hi" }, "queue1"));
            queueProvider.Verify(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"), Times.Exactly(5));
        }

        [Fact]
        public void SendObject()
        {
            var queueProvider = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(queueProvider.Object);
            senderEntityDefault.Send<QueueMessageDefault>("Hi", "System.String", "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == null), "queue1"));
        }

        [Fact]
        public void SendObjectWithDelay()
        {
            var queueProvider = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(queueProvider.Object);
            senderEntityDefault.Send<QueueMessageDefault>("Hi", "System.String", 10, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == 10), "queue1"));
        }

        [Fact]
        public void SendObjectList()
        {
            var queueProvider = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(queueProvider.Object);
            senderEntityDefault.SendList<QueueMessageDefault>(new[] { "Hi", "Bye" }, "System.String", 10, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == 10), "queue1"));
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Bye\"" && y.Delay == 10), "queue1"));
        }

        [Fact]
        public void Dispose()
        {
            var queueProvider = GetMocks();
            using (var senderEntityDefault = new SenderEntityDefault(queueProvider.Object))
            {
            };
            queueProvider.Verify(x => x.Dispose());
        }

        private Mock<IQueueProvider> GetMocks()
        {
            var queueProvider = new Mock<IQueueProvider>();
            return queueProvider;
        }
    }
}
