namespace bgTeam.Impl.Kafka
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Threading.Tasks.Dataflow;
    using Confluent.Kafka;

    public class MutlithreadKafkaQueueWatcher<TMessage> : KafkaQueueWatcher<TMessage>
        where TMessage : IKafkaMessage
    {
        private readonly ActionBlock<ConsumeResult<byte[], byte[]>> _mainPipe;
        private readonly BatchBlock<(TopicPartition Partition, Offset Offset)> _batchBlockToCommit;
        private readonly Timer _timer;

        private bool _disposed = false;

        public MutlithreadKafkaQueueWatcher(IMultithreadKafkaSettings kafkaSettings)
            : base(kafkaSettings)
        {
            var mainPipeConfig = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = kafkaSettings.ThreadCount,
                BoundedCapacity = kafkaSettings.BoundedCapacity,
            };
            _mainPipe = new ActionBlock<ConsumeResult<byte[], byte[]>>(HandleMessageInternalAsync, mainPipeConfig);

            var commitPipe = new ActionBlock<(TopicPartition Partition, Offset Offset)[]>(BulkCommit);
            _batchBlockToCommit = new BatchBlock<(TopicPartition Partition, Offset Offset)>(kafkaSettings.BoundedCapacity);
            _batchBlockToCommit.LinkTo(commitPipe);

            _timer = new Timer((x) => _batchBlockToCommit.TriggerBatch(), null, kafkaSettings.CommitIntervalMs, kafkaSettings.CommitIntervalMs);
        }

        protected override Task HandleMessageAsync(ConsumeResult<byte[], byte[]> consumeResult)
        {
            return _mainPipe.SendAsync(consumeResult);
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _timer?.Dispose();
            }

            _disposed = true;

            base.Dispose(disposing);
        }

        private async Task HandleMessageInternalAsync(ConsumeResult<byte[], byte[]> consumeResult)
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
                Commit(consumeResult);
            }
        }

        private void BulkCommit((TopicPartition Partition, Offset Offset)[] toCommit)
        {
            var partitionOffsets = toCommit
                .GroupBy(x => x.Partition)
                .Select(x => new TopicPartitionOffset(x.Key, x.Max(o => o.Offset)));

            Commit(partitionOffsets);
        }
    }
}
