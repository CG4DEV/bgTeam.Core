namespace bgTeam.Impl.Rabbit
{
    using bgTeam.Extensions;
    using bgTeam.Queues;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class QueueProviderRabbitMQ : IQueueProvider
    {
        private readonly string EXCHANGE_DEFAULT = "bgTeam.direct";

        private readonly bool _useDelay;
        private readonly List<string> _queues;
        private readonly IAppLogger _logger;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        private static readonly object _locker = new object();

        public QueueProviderRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IQueueProviderSettings settings,
            bool useDelay = false,
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

            if (queues.NullOrEmpty())
            {
                throw new ArgumentNullException("queues");
            }

            _useDelay = useDelay;
            _queues = queues.ToList();

            if (_useDelay)
            {
                EXCHANGE_DEFAULT = $"{EXCHANGE_DEFAULT}.delay";
            }

            Init(queues);
        }

        public void PushMessage(IQueueMessage message)
        {
            PushMessageInternal(_queues, message);
        }

        public void PushMessage(IQueueMessage message, params string[] queues)
        {
            queues = GetDistinctQueues(queues);
            PushMessageInternal(queues, message);
        }

        private string[] GetDistinctQueues(string[] queues)
        {
            queues = queues.CheckNullOrEmpty(nameof(queues)).Distinct().ToArray();

            var queuesToSend = queues
                        .Where(x => _queues.Any(q => q.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                        .ToArray();

            if (queuesToSend.Count() != queues.Count())
            {
                lock (_locker)
                {
                    var queuesToSend2 = queues
                            .Where(x => _queues.Any(q => q.Equals(x, StringComparison.InvariantCultureIgnoreCase)))
                            .ToArray();

                    if (queuesToSend2.Count() != queues.Count())
                    {
                        var toInitQueues = queues.Except(queuesToSend2).ToArray();
                        Init(toInitQueues);
                        _queues.AddRange(toInitQueues);
                    }
                }
            }

            return queues;
        }

        private void PushMessageInternal(IEnumerable<string> queues, params IQueueMessage[] messages)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                foreach (var message in messages)
                {
                    var body = _msgProvider.PrepareMessageByte(message);

                    foreach (var item in queues)
                    {
                        var bProp = channel.CreateBasicProperties();
                        var bHeaders = new Dictionary<string, object>();

                        bHeaders.Add("x-delay", message.Delay);
                        bProp.Headers = bHeaders;
                        bProp.DeliveryMode = 2;

                        channel.BasicPublish(EXCHANGE_DEFAULT, item, bProp, body);
                    }
                }
            }
        }

        public void PushMessages(IEnumerable<IQueueMessage> messages)
        {
            if (messages.NullOrEmpty())
            {
                return;
            }

            PushMessageInternal(_queues, messages.ToArray());
        }

        public void PushMessages(IEnumerable<IQueueMessage> messages, params string[] queues)
        {
            if (messages.NullOrEmpty())
            {
                return;
            }

            queues = GetDistinctQueues(queues);
            PushMessageInternal(queues, messages.ToArray());
        }

        public QueueMessageWork AskMessage(string queueName)
        {
            throw new NotImplementedException();
        }

        public void DeleteMessage(QueueMessageWork message)
        {
            throw new NotImplementedException();
        }

        public uint GetQueueMessageCount(string queueName)
        {
            var queue = _queues.SingleOrDefault(x => x.Equals(queueName, StringComparison.InvariantCultureIgnoreCase));
            if (queue == null)
            {
                throw new Exception($"Не найдена очередь с именем {queueName}");
            }

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                return channel.MessageCount(queue);
            }
        }

        /// <summary>
        /// Проверяем что очередь создана.
        /// </summary>
        private void Init(IEnumerable<string> queues)
        {
            _logger.Debug($"QueueProviderRabbitMQ: create connect to {string.Join(", ", queues)}");

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _logger.Debug($"QueueProviderRabbitMQ: connect open");

                foreach (var item in queues)
                {
                    if (_useDelay)
                    {
                        // при подключении плагина на задержку времени
                        var args = new Dictionary<string, object> { { "x-delayed-type", "direct" } };
                        channel.ExchangeDeclare(EXCHANGE_DEFAULT, "x-delayed-message", true, false, args);
                    }
                    else
                    {
                        // без плагина
                        channel.ExchangeDeclare(EXCHANGE_DEFAULT, "direct", true, false, null);
                    }

                    var queue = channel.QueueDeclare(item, true, false, false, null);
                    channel.QueueBind(queue, EXCHANGE_DEFAULT, item);
                }
            }
        }
    }
}
