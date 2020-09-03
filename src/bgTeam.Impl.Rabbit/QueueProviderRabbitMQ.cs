namespace bgTeam.Impl.Rabbit
{
    using bgTeam;
    using bgTeam.Extensions;
    using bgTeam.Queues;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Желательное использование одного провайдера на поток
    /// </summary>
    public class QueueProviderRabbitMQ : IQueueProvider
    {
        private bool _disposed = false;
        private readonly string EXCHANGE_DEFAULT = "bgTeam.direct";

        private readonly bool _useDelay;
        private List<string> _queues;
        private IAppLogger _logger;
        private IConnectionFactory _factory;
        private IMessageProvider _msgProvider;

        private static readonly object _locker = new object();
        private static readonly object _lockChannel = new object();

        private IModel _channel;

        public QueueProviderRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory,
            bool useDelay = false,
            params string[] queues)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _msgProvider = msgProvider ?? throw new ArgumentNullException(nameof(msgProvider));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            if (queues.NullOrEmpty())
            {
                throw new ArgumentNullException(nameof(queues));
            }

            _useDelay = useDelay;
            _queues = queues.ToList();

            if (_useDelay)
            {
                EXCHANGE_DEFAULT = $"{EXCHANGE_DEFAULT}.delay";
            }

            Init(queues);
        }

        ~QueueProviderRabbitMQ()
        {
            Dispose(false);
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

        public uint GetQueueMessageCount(string queueName)
        {
            var queue = _queues.SingleOrDefault(x => x.Equals(queueName, StringComparison.InvariantCultureIgnoreCase));
            if (queue == null)
            {
                throw new Exception($"Не найдена очередь с именем {queueName}");
            }

            using (var channel = CreateChannel())
            {
                return channel.MessageCount(queue);
            }
        }

        public void Dispose()
        {
            Dispose(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && _channel != null)
                {
                    // Освобождаем управляемые ресурсы
                    _channel.Close();
                    _channel.Dispose();
                }

                // освобождаем неуправляемые объекты
                _channel = null;
                _logger = null;
                _factory = null;
                _msgProvider = null;
                _queues = null;

                _disposed = true;
            }
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

        private void PushMessageInternal(IEnumerable<string> queues, IQueueMessage message)
        {
            var channel = CreateChannel();
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

        /// <summary>
        /// Проверяем что очередь создана.
        /// </summary>
        private void Init(IEnumerable<string> queues)
        {
            _logger.Debug($"QueueProviderRabbitMQ: create connect to {string.Join(", ", queues)}");

            using (var channel = CreateChannel())
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

        private IModel CreateChannel()
        {
            if (_channel == null || !_channel.IsOpen)
            {
                //Лочим на всякий, вдруг один экземпляр провайдера попадет в несколько потоков
                lock (_lockChannel)
                {
                    if (_channel == null || !_channel.IsOpen)
                    {
                        _channel = _factory.CreateConnection().CreateModel();
                    }
                }
            }

            return _channel;
        }
    }
}
