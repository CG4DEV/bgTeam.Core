namespace bgTeam.DataProducerService.Infrastructure
{
    using bgTeam.Queues;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueueProviderRabbitMQ : IQueueProvider
    {
        private const string EXCHANGE_DEFAULT = "bgTeam.direct";

        private readonly string[] _queues;
        private readonly IAppLogger _logger;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        public QueueProviderRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IQueueProviderSettings settings,
            params string[] queues)
        {
            _logger = logger;
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

            Init(queues);
        }

        public void PushMessage(IQueueMessage message)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var body = _msgProvider.PrepareMessageByte(message);

                foreach (var item in _queues)
                {
                    var bProp = channel.CreateBasicProperties();
                    var bHeaders = new Dictionary<string, object>();

                    bHeaders.Add("x-delay", message.Delay);
                    bProp.Headers = bHeaders;

                    ////channel.BasicPublish(routingKey: item, body: body, exchange: string.Empty, basicProperties: bProp);
                    channel.BasicPublish(EXCHANGE_DEFAULT, item, bProp, body);
                }
            }
        }

        public QueueMessageWork AskMessage(string queueName)
        {
            ////using (var connection = _factory.CreateConnection())
            ////using (var channel = connection.CreateModel())
            ////{
            ////    var res = channel.BasicGet(queueName, false);

            ////    var body = _msgProvider.ExtractObject(res.Body);

            ////    return new QueueMessageWork()
            ////    {
            ////        Id = res.DeliveryTag,
            ////        Body = body.Body,
            ////    };
            ////}

            throw new NotImplementedException();
        }

        public void DeleteMessage(QueueMessageWork message)
        {
            ////using (var connection = _factory.CreateConnection())
            ////using (var channel = connection.CreateModel())
            ////{
            ////    channel.BasicAck(message.Id, false);
            ////}

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

                return channel.MessageCount(queue);
            }
        }

        /// <summary>
        /// Проверяем что очередь создана
        /// </summary>
        private void Init(string[] queues)
        {
            _logger.Debug($"QueueProviderRabbitMQ: create connect to { string.Join(", ", queues)}");

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _logger.Debug($"QueueProviderRabbitMQ: connect open");

                foreach (var item in _queues)
                {
                    ////channel.QueueDeclare(queue: item, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    ////var args = new Dictionary<string, object> { { "x-delayed-type", "direct" } };
                    ////channel.ExchangeDeclare(item, "x-delayed-message", true, false, args);

                    channel.ExchangeDeclare(EXCHANGE_DEFAULT, "direct", true, false, null);
                    var queue = channel.QueueDeclare(item, true, false, false, null);
                    channel.QueueBind(queue, EXCHANGE_DEFAULT, item);
                }
            }
        }
    }
}
