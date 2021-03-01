namespace bgTeam.Impl.Kafka
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Extensions;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using Confluent.Kafka;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class KafkaQueueWatcher<TMessage> : IQueueWatcher<TMessage>, IDisposable
        where TMessage : IKafkaMessage
    {
        private const int COMMIT_PERIOD = 10;

        private readonly ILogger<KafkaQueueWatcher<TMessage>> _logger;
        private readonly IKafkaSettings _kafkaSettings;

        private IConsumer<byte[], byte[]> _consumer;
        private string _topic;
        private Task _mainLoop;
        private CancellationTokenSource _cts;
        private bool _disposedValue;

        public KafkaQueueWatcher(
            ILogger<KafkaQueueWatcher<TMessage>> logger,
            IKafkaSettings kafkaSettings)
        {
            _logger = logger;
            _kafkaSettings = kafkaSettings;
        }

        public event QueueMessageHandler Subscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> Error;

        protected ILogger<KafkaQueueWatcher<TMessage>> Logger => _logger;

        protected IKafkaSettings KafkaSettings => _kafkaSettings;

        public void StartWatch(string queueName)
        {
            _topic = queueName.CheckNull(nameof(queueName));

            if (Subscribe == null)
            {
                throw new QueueException("No subscribers");
            }

            Close();

            _consumer = BuildConsumer(_kafkaSettings);
            _cts = new CancellationTokenSource();

            _consumer.Subscribe(_topic);
            _mainLoop = Task.Factory.StartNew(MainLoop, _cts.Token);
        }

        public void Close()
        {
            if (_consumer != null)
            {
                _cts.Cancel();
                _consumer.Close();
                _consumer = null;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _cts?.Dispose();
                    _consumer?.Dispose();
                }

                _cts = null;
                _consumer = null;
                _disposedValue = true;
            }
        }

        protected virtual async Task HandleMessageAsync(ConsumeResult<byte[], byte[]> consumeResult)
        {
            var message = Deserialize(consumeResult);
            try
            {
                await OnSubscribe(message);
            }
            catch (Exception ex)
            {
                OnError(message, ex);
            }
            finally
            {
                if (consumeResult.Offset % COMMIT_PERIOD == 0)
                {
                    Commit(consumeResult);
                }
            }
        }

        protected virtual IKafkaMessage Deserialize(ConsumeResult<byte[], byte[]> consumeResult)
        {
            var msg = consumeResult.Message;
            var kafkaMessage = JsonConvert.DeserializeObject<KafkaMessage>(Encoding.UTF8.GetString(msg.Value));

            kafkaMessage.Key = Encoding.UTF8.GetString(msg.Key);
            kafkaMessage.Partition = consumeResult.Partition.Value;
            kafkaMessage.Offset = consumeResult.Offset.Value;

            return kafkaMessage;
        }

        protected virtual void Commit(ConsumeResult<byte[], byte[]> consumeResult)
        {
            Commit(new[] { new TopicPartitionOffset(consumeResult.TopicPartition, consumeResult.Offset + 1) });
        }

        protected void Commit(IEnumerable<TopicPartitionOffset> offsets)
        {
            _consumer.Commit(offsets);
        }

        protected Task OnSubscribe(IKafkaMessage kafkaMessage)
        {
            return Subscribe?.Invoke(kafkaMessage);
        }

        protected void OnError(IKafkaMessage kafkaMessage, Exception ex)
        {
            Error?.Invoke(this, new ExtThreadExceptionEventArgs(kafkaMessage, ex));
        }

        private async Task MainLoop()
        {
            try
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = _consumer.Consume(_cts.Token);

                        if (consumeResult.IsPartitionEOF)
                        {
                            _logger.LogInformation(
                                "Reached end of topic {topic}, partition {partition}, offset {offset}",
                                _topic,
                                consumeResult.Partition,
                                consumeResult.Offset);

                            continue;
                        }

                        await HandleMessageAsync(consumeResult);
                    }
                    catch (ConsumeException cex)
                    {
                        _logger.LogError(cex, "Consume error: {consumerError}", cex.Error.Reason);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Closing consume topic {topic}.", _topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal consumer error: {consumerError}", ex.Message);
            }
        }

        private IConsumer<byte[], byte[]> BuildConsumer(IKafkaSettings kafkaSettings)
        {
            return new ConsumerBuilder<byte[], byte[]>(kafkaSettings.Config)
                .SetKeyDeserializer(Deserializers.ByteArray)
                .SetValueDeserializer(Deserializers.ByteArray)
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
