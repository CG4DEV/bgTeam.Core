using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class QueueProviderRabbitMQTests
    {
        [Fact]
        public void DependencyMessageProvider()
        {
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("msgProvider", () =>
            {
                new QueueProviderRabbitMQ(null, connectionFactory.Object);
            });
        }

        [Fact]
        public void DependencyConnectionFactory()
        {
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("factory", () =>
            {
                new QueueProviderRabbitMQ(messageProvider.Object, null);
            });
        }

        [Fact]
        public void DependencyQueues()
        {
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("queues", () =>
            {
                new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object);
            });
        }

        [Fact]
        public void InitWithDelay()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var connection = new Mock<IConnection>();
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            model.Verify(x => x.QueueBind("queue1", "bgTeam.direct.delay", "queue1", null));
            model.Verify(x => x.ExchangeDeclare("bgTeam.direct.delay", "x-delayed-message", true, false, It.IsAny<IDictionary<string, object>>()));
        }

        [Fact]
        public void InitWithoutDelay()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var connection = new Mock<IConnection>();
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, false, "queue1");
            model.Verify(x => x.QueueBind("queue1", "bgTeam.direct", "queue1", null));
            model.Verify(x => x.ExchangeDeclare("bgTeam.direct", "direct", true, false, null));
        }

        [Fact]
        public void PushMessage()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            queueProviderRabbitMQ.PushMessage(new QueueMessageDefault() { Body = "hi" });
            model.Verify(x => x.BasicPublish("bgTeam.direct.delay", "queue1", false, It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));
        }

        [Fact]
        public void PushMessageInQueues()
        {
            var model = new Mock<IModel>();
            model.Setup(x => x.QueueDeclare("queue2", It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null))
                .Returns(new QueueDeclareOk("queue2", 12, 2));
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            queueProviderRabbitMQ.PushMessage(new QueueMessageDefault() { Body = "hi" }, "queue1", "queue2");
            model.Verify(x => x.BasicPublish("bgTeam.direct.delay", "queue1", false, It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));
            model.Verify(x => x.BasicPublish("bgTeam.direct.delay", "queue2", false, It.IsAny<IBasicProperties>(), It.IsAny<ReadOnlyMemory<byte>>()));
        }

        [Fact]
        public void GetQueueMessageCount()
        {
            var model = new Mock<IModel>();
            model.Setup(x => x.MessageCount("queue1"))
                .Returns(10);
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            var count = queueProviderRabbitMQ.GetQueueMessageCount("queue1");
            Assert.Equal((uint)10, count);
        }

        [Fact(Skip = "When run with one thread - this test will be hovered")]
        public void DisposeShouldCloseChannel()
        {
            var model = new Mock<IModel>();
            var @event = new ManualResetEvent(false);

            model.Setup(x => x.MessageCount("queue1"))
                .Returns(() =>
                {
                    @event.WaitOne();
                    return 10;
                });

            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);

            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            var task = Task.Factory.StartNew(() => queueProviderRabbitMQ.GetQueueMessageCount("queue1"));

            queueProviderRabbitMQ.Dispose();
            model.Verify(x => x.Close());
            model.Verify(x => x.Dispose());

            Assert.Throws<AggregateException>(() => task.Wait());
            @event.Set();
        }

        [Fact]
        public void GetQueueMessageCountShouldThrowsExceptionIfQueueNotExists()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueProviderRabbitMQ = new QueueProviderRabbitMQ(messageProvider.Object, connectionFactory.Object, true, "queue1");
            Assert.Throws<Exception>(() => queueProviderRabbitMQ.GetQueueMessageCount("queue2"));
        }
    }
}
