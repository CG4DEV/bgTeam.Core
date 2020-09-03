using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using System.Text;

namespace bgTeam.Core.Tests.Rabbit
{
    public static class RabbitMockFactory
    {
        public static (
            Mock<IAppLogger>,
            Mock<IMessageProvider>,
            Mock<IQueueProviderSettings>,
            Mock<IConnectionFactory>)
            Get(Mock<IModel> model = null)
        {
            var appLogger = new Mock<IAppLogger>();
            var messageProvider = new Mock<IMessageProvider>();
            var queueProviderSettings = new Mock<IQueueProviderSettings>();
            var connectionFactory = new Mock<IConnectionFactory>();


            messageProvider.Setup(x => x.ExtractObject(It.IsAny<byte[]>()))
                .Returns((byte[] source) =>
                {
                    var message = new Mock<IQueueMessage>();
                    message.SetupGet(x => x.Body)
                        .Returns(Encoding.ASCII.GetString(source ?? new byte[0]));
                    return message.Object;
                });

            messageProvider.Setup(x => x.ExtractObject(It.IsAny<string>()))
                .Returns((string source) =>
                {
                    var message = new Mock<IQueueMessage>();
                    message.SetupGet(x => x.Body)
                        .Returns(source);
                    return message.Object;
                });

            var connection = new Mock<IConnection>();
            if (model == null)
            {
                model = new Mock<IModel>();
            }

            var basicProperties = new Mock<IBasicProperties>();

            model.Setup(x => x.BasicGet("queue1", It.IsAny<bool>()))
                .Returns(new BasicGetResult(110, false, "", "", 9, basicProperties.Object, Encoding.ASCII.GetBytes("Hi")));
            model.Setup(x => x.QueueDeclare("queue1", It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), null))
                .Returns(new QueueDeclareOk("queue1", 122, 3));
            model.Setup(x => x.CreateBasicProperties())
               .Returns(new Mock<IBasicProperties>().Object);
            

            connection.Setup(x => x.CreateModel())
                .Returns(model.Object);
            connectionFactory.Setup(x => x.CreateConnection())
                .Returns(connection.Object);

            return (
                appLogger,
                messageProvider,
                queueProviderSettings,
                connectionFactory);
        }
    }
}
