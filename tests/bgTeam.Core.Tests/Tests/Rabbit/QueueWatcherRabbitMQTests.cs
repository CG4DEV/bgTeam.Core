using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class QueueWatcherRabbitMQTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new QueueWatcherRabbitMQ(null, messageProvider.Object, queueProviderSettings.Object);
            });
        }

        [Fact]
        public void DependencyMessageProvider()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("msgProvider", () =>
            {
                new QueueWatcherRabbitMQ(appLogger.Object, null, queueProviderSettings.Object);
            });
        }

        [Fact]
        public void DependencyQueueProviderSettings()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("settings", () =>
            {
                new QueueWatcherRabbitMQ(appLogger.Object, messageProvider.Object, (IQueueProviderSettings)null);
            });
        }

        [Fact]
        public void DependencyConnectionFactory()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            Assert.Throws<ArgumentNullException>("factory", () =>
            {
                new QueueWatcherRabbitMQ(appLogger.Object, messageProvider.Object, (RabbitMQ.Client.IConnectionFactory)null);
            });
        }

        [Fact]
        public void DependencySubscribe()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            var queueWatcherRabbitMQ = new QueueWatcherRabbitMQ(appLogger.Object, messageProvider.Object, queueProviderSettings.Object);
            Assert.Throws<ArgumentNullException>("Subscribe", () =>
            {
                queueWatcherRabbitMQ.StartWatch("queue1");
            });
        }

        [Fact]
        public void StartWatch()
        {
            var (appLogger, messageProvider, queueProviderSettings, connectionFactory) = RabbitMockFactory.Get();
            var queueWatcherRabbitMQ = new QueueWatcherRabbitMQ(appLogger.Object, messageProvider.Object, connectionFactory.Object);

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
