namespace bgTeam.Impl.Kafka
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Extensions;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using Confluent.Kafka;
    using Microsoft.Extensions.Logging;

    public class BaseKafkaQueueWatcher<TQueueMessage> : IQueueWatcher<TQueueMessage>
        where TQueueMessage : IQueueMessage
    {
        private readonly ILogger<BaseKafkaQueueWatcher<TQueueMessage>> _logger;
        private readonly IMessageProvider _messageProvider;
        private readonly IKafkaSettings _kafkaSettings;

        private IConsumer<byte[], byte[]> _consumer;
        private string _topic;
        private Task _mainLoop;
        private CancellationTokenSource _cts;

        public BaseKafkaQueueWatcher(
            ILogger<BaseKafkaQueueWatcher<TQueueMessage>> logger,
            IMessageProvider messageProvider,
            IKafkaSettings kafkaSettings)
        {
            _logger = logger;
            _messageProvider = messageProvider;
            _kafkaSettings = kafkaSettings;
        }

        public event QueueMessageHandler Subscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> Error;

        protected ILogger<BaseKafkaQueueWatcher<TQueueMessage>> Logger => _logger;

        protected IMessageProvider MessageProvider => _messageProvider;

        protected IKafkaSettings KafkaSettings => _kafkaSettings;

        public void StartWatch(string queueName)
        {
            _topic = queueName.CheckNull(nameof(queueName));

            if (_consumer != null)
            {
                _cts.Cancel();
                _consumer.Close();
            }

            _consumer = BuildConsumer(_kafkaSettings);
            _cts = new CancellationTokenSource();

            _consumer.Subscribe(_topic);
            _mainLoop = Task.Factory.StartNew(MainLoop, _cts.Token);
        }

        private Task MainLoop()
        {
            return Task.CompletedTask;
        }

        private IConsumer<byte[], byte[]> BuildConsumer(IKafkaSettings kafkaSettings)
        {
            return new ConsumerBuilder<byte[], byte[]>(kafkaSettings.Config)
                   .SetErrorHandler(HandleError)
                   .SetLogHandler(HandleLog)
                   .Build();
        }

        private void HandleError(IConsumer<byte[], byte[]> consumer, Error error)
        {
            _logger.LogError("Kafka error. {message}. By topics: {topics}", error.Reason, _topic);
        }

        private void HandleLog(IConsumer<byte[], byte[]> consumer, LogMessage logMessage)
        {
            _logger.LogInformation("Kafka info. {message}. By topics: {topics}", logMessage.Message, _topic);
        }
    }
}
