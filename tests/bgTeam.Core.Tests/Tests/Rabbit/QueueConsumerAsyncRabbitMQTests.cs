using System;
using System.Reflection;
using System.Threading.Tasks;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class QueueConsumerAsyncRabbitMQTests
    {
        [Fact]
        public async Task ReceiverHandler()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueConsumerRabbitMQ = new QueueConsumerAsyncRabbitMQ(connectionFactory.Object, messageProvider.Object);

            queueConsumerRabbitMQ.Subscribe += (x) =>
            {
                return Task.FromResult(new QueueMessageDefault("Hi"));
            };

            // Get AsyncEventingBasicConsumer from private field
            var _consumer = (AsyncEventingBasicConsumer)typeof(QueueConsumerAsyncRabbitMQ).GetField("_consumer", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(queueConsumerRabbitMQ);

            // Raise event in AsyncEventingBasicConsumer
            var @event = (AsyncEventHandler<BasicDeliverEventArgs>)typeof(AsyncEventingBasicConsumer).GetField("Received", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_consumer);
            await @event.Invoke(new AsyncEventingBasicConsumer(model.Object), new BasicDeliverEventArgs());

            model.Verify(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));
        }

        [Fact]
        public async Task ReceiverHandlerWithException()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueConsumerRabbitMQ = new QueueConsumerAsyncRabbitMQ(connectionFactory.Object, messageProvider.Object);

            queueConsumerRabbitMQ.Subscribe += (x) =>
            {
                throw new Exception();
            };

            Exception exception = null;
            queueConsumerRabbitMQ.Error += (obj, ex) =>
            {
                exception = ex.Exception;
            };

            var _consumer = (AsyncEventingBasicConsumer)typeof(QueueConsumerAsyncRabbitMQ).GetField("_consumer", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(queueConsumerRabbitMQ);
            var @event = (AsyncEventHandler<BasicDeliverEventArgs>)typeof(AsyncEventingBasicConsumer).GetField("Received", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_consumer);
            await @event.Invoke(new AsyncEventingBasicConsumer(model.Object), new BasicDeliverEventArgs());

            Assert.NotNull(exception);
            model.Verify(x => x.BasicAck(It.IsAny<ulong>(), It.IsAny<bool>()));
        }

        [Fact]
        public async Task ShutdownHandler()
        {
            var model = new Mock<IModel>();
            var (messageProvider, _, connectionFactory) = RabbitMockFactory.Get(model);
            var queueConsumerRabbitMQ = new QueueConsumerAsyncRabbitMQ(connectionFactory.Object, messageProvider.Object);

            queueConsumerRabbitMQ.Subscribe += (x) =>
            {
                return Task.FromResult(new QueueMessageDefault("Hi"));
            };

            var _consumer = (AsyncEventingBasicConsumer)typeof(QueueConsumerAsyncRabbitMQ).GetField("_consumer", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(queueConsumerRabbitMQ);
            var @event = (AsyncEventHandler<ShutdownEventArgs>)typeof(AsyncEventingBasicConsumer).GetField("Shutdown", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(_consumer);
            await @event.Invoke(new AsyncEventingBasicConsumer(model.Object), new ShutdownEventArgs(ShutdownInitiator.Application, 0, ""));
        }
    }
}
