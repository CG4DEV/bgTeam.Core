using bgTeam.Queues;
using bgTeam.Queues.Impl;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues.Impl
{
    public class SenderEntityDefaultTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, queueProvider) = GetMocks();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new SenderEntityDefault(null, queueProvider.Object);
            });
        }

        [Fact]
        public void DependencyQueueProvider()
        {
            var (appLogger, queueProvider) = GetMocks();
            Assert.Throws<ArgumentNullException>("queueProvider", () =>
            {
                new SenderEntityDefault(appLogger.Object, null);
            });
        }

        [Fact]
        public void SendEntity()
        {
            var (appLogger, queueProvider) = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object);
            senderEntityDefault.Send(new QueueMessageDefault { Body = "Hi" }, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"));
        }

        [Fact]
        public void SendEntityWithExceptionShouldRetryAttemption5Times()
        {
            var (appLogger, queueProvider) = GetMocks();
            queueProvider.Setup(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"))
                .Throws(new Exception());
            var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object);
            senderEntityDefault.Send(new QueueMessageDefault { Body = "Hi" }, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.IsAny<IQueueMessage>(), "queue1"), Times.Exactly(5));
        }

        [Fact]
        public void SendObject()
        {
            var (appLogger, queueProvider) = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object);
            senderEntityDefault.Send<QueueMessageDefault>("Hi", "System.String", "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == null), "queue1"));
        }

        [Fact]
        public void SendObjectWithDelay()
        {
            var (appLogger, queueProvider) = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object);
            senderEntityDefault.Send<QueueMessageDefault>("Hi", "System.String", 10, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == 10), "queue1"));
        }

        [Fact]
        public void SendObjectList()
        {
            var (appLogger, queueProvider) = GetMocks();
            var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object);
            senderEntityDefault.SendList<QueueMessageDefault>(new[] { "Hi", "Bye" }, "System.String", 10, "queue1");
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Hi\"" && y.Delay == 10), "queue1"));
            queueProvider.Verify(x => x.PushMessage(It.Is<IQueueMessage>(y => y.Body == "\"Bye\"" && y.Delay == 10), "queue1"));
        }

        [Fact]
        public void Dispose()
        {
            var (appLogger, queueProvider) = GetMocks();
            using (var senderEntityDefault = new SenderEntityDefault(appLogger.Object, queueProvider.Object))
            {
            };
            queueProvider.Verify(x => x.Dispose());
        }

        private (
            Mock<IAppLogger>,
            Mock<IQueueProvider>)
            GetMocks()
        {
            var appLogger = new Mock<IAppLogger>();
            var queueProvider = new Mock<IQueueProvider>();
            return (
                appLogger,
                queueProvider);
        }
    }
}
