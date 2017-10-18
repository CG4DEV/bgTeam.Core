namespace bgTeam.DataProducerService.Infrastructure
{
    using bgTeam.Queues;
    using RabbitMQ.Client;
    using System;
    using System.Linq;

    public class QueueProviderRabbitMQ : IQueueProvider
    {
        private readonly string[] _queues;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        public QueueProviderRabbitMQ(IMessageProvider msgProvider, IQueueProviderSettings settings, params string[] queues)
        {
            _msgProvider = msgProvider;
            _factory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = settings.VirtualHost,
                UserName = settings.Login,
                Password = settings.Password,
            };

            if (queues == null || queues.Length == 0)
            {
                throw new ArgumentNullException("queues");
            }

            _queues = queues;

            Init();
        }

        /// <summary>
        /// Проверяем что очередь создана
        /// </summary>
        private void Init()
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                foreach (var item in _queues)
                {
                    channel.QueueDeclare(queue: item, durable: true, exclusive: false, autoDelete: false, arguments: null);
                }
            }
        }

        public void PushMessage(IQueueMessage message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = _msgProvider.PrepareMessageByte(message);

                foreach (var item in _queues)
                {
                    IBasicProperties basicProperties = channel.CreateBasicProperties();

                    channel.BasicPublish(routingKey: item, body: body, exchange: string.Empty, basicProperties: null);
                }
            }
        }

        public QueueMessageWork AskMessage(string queueName)
        {
            //using (var connection = _factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    var res = channel.BasicGet(queueName, false);

            //    var body = _msgProvider.ExtractObject(res.Body);

            //    return new QueueMessageWork()
            //    {
            //        Id = res.DeliveryTag,
            //        Body = body.Body,
            //    };
            //}

            throw new NotImplementedException();
        }

        public void DeleteMessage(QueueMessageWork message)
        {
            //using (var connection = _factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.BasicAck(message.Id, false);
            //}

            throw new NotImplementedException();
        }

        public uint GetQueueMessageCount(string queueName)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var queue = _queues.SingleOrDefault(x => x.Equals(queueName, StringComparison.InvariantCultureIgnoreCase));
                if (queue == null)
                {
                    throw new Exception($"Не найдена очередь с именем {queueName}");
                }

                IBasicProperties basicProperties = channel.CreateBasicProperties();
                return channel.MessageCount(queue);
            }
        }
    }
}
