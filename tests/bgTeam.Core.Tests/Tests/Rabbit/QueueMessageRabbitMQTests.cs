using bgTeam.Impl.Rabbit;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class QueueMessageRabbitMQTests
    {
        [Fact]
        public void QueueMessageRabbitMQEmptyConstructor()
        {
            var queueMessageRabbitMQ = new QueueMessageRabbitMQ();
            Assert.NotNull(queueMessageRabbitMQ.Errors);
        }

        [Fact]
        public void QueueMessageRabbitMQ()
        {
            var queueMessageRabbitMQ = new QueueMessageRabbitMQ("hi");
            Assert.NotNull(queueMessageRabbitMQ.Errors);
            Assert.Equal("hi", queueMessageRabbitMQ.Body);
        }

        [Fact]
        public void Delay()
        {
            var queueMessageRabbitMQ = new QueueMessageRabbitMQ("hi");
            queueMessageRabbitMQ.Errors.Add("error1");
            queueMessageRabbitMQ.Errors.Add("error2");
            queueMessageRabbitMQ.Errors.Add("error3");

            var delay = queueMessageRabbitMQ.Delay;
            Assert.Equal(7200000, delay);

            queueMessageRabbitMQ.Delay = 123;
            Assert.Equal(7200000, delay);
        }
    }
}
