using System;
using System.Threading;
using System.Threading.Tasks;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class QueueWatcherRabbitMQTests
    {
        [Fact]
        public void DependencyMessageProvider()
        {
            var (messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("msgProvider", () =>
            {
                new QueueWatcherRabbitMQ(null, queueProviderSettings.Object);
            });
        }

        [Fact]
        public void DependencyQueueProviderSettings()
        {
            var (messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("settings", () =>
            {
                new QueueWatcherRabbitMQ(messageProvider.Object, (IQueueProviderSettings)null);
            });
        }

        [Fact]
        public void DependencyConnectionFactory()
        {
            var (messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("factory", () =>
            {
                new QueueWatcherRabbitMQ(messageProvider.Object, (RabbitMQ.Client.IConnectionFactory)null);
            });
        }

        [Fact]
        public void DependencySubscribe()
        {
            var (messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            var queueWatcherRabbitMQ = new QueueWatcherRabbitMQ(messageProvider.Object, queueProviderSettings.Object);
            Assert.Throws<ArgumentNullException>("Subscribe", () =>
            {
                queueWatcherRabbitMQ.StartWatch("queue1");
            });
        }

        [Fact]
        public void StartWatch()
        {
            var (messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            var queueWatcherRabbitMQ = new QueueWatcherRabbitMQ(messageProvider.Object, connectionFactory.Object);

            var @event = new ManualResetEvent(false);
            IQueueMessage message = null;

            queueWatcherRabbitMQ.Subscribe += (IQueueMessage mess) => Task.Run(() =>
            {
                message = mess;
                @event.Set();
            });

            Task.Factory.StartNew(() => queueWatcherRabbitMQ.StartWatch("queue1"));
            @event.WaitOne();

            Assert.NotNull(message);
            Assert.Equal("Hi", message.Body);
        }
    }
}
