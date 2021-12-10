namespace bgTeam.Impl.Rabbit
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    public class QueueConsumerAsyncRabbitMQ : BaseQueueConsumerRabbitMQ<AsyncEventingBasicConsumer>, IQueueWatcher<IQueueMessage>
    {
        public QueueConsumerAsyncRabbitMQ(IConnectionFactory connectionFactory, IMessageProvider provider)
            : this(connectionFactory, provider, PREFETCHCOUNT)
        {
        }

        public QueueConsumerAsyncRabbitMQ(
            IConnectionFactory connectionFactory,
            IMessageProvider provider,
            ushort prefetchCount)
            : base(connectionFactory, provider, prefetchCount)
        {
        }

        protected override void InitConsumer()
        {
            if (_consumer != null)
            {
                _consumer.Received -= ReceiverHandler;
                _consumer.Shutdown -= ShutdownHandler;
            }

            _consumer = new AsyncEventingBasicConsumer(_model);
            _consumer.Received += ReceiverHandler;
            _consumer.Shutdown += ShutdownHandler;
        }

        private async Task ReceiverHandler(object sender, BasicDeliverEventArgs e)
        {
            var message = _provider.ExtractObject(Encoding.UTF8.GetString(e.Body.ToArray()));
            try
            {
                await OnSubscribe(message);
            }
            catch (Exception ex)
            {
                OnError(this, new ExtThreadExceptionEventArgs(message, ex));
            }
            finally
            {
                var model = ((AsyncEventingBasicConsumer)sender).Model;
                model.BasicAck(e.DeliveryTag, false);
            }
        }

        private Task ShutdownHandler(object sender, ShutdownEventArgs e)
        {
            InitConsumer();
            StartWatch(_watchingQueueName);

            return Task.CompletedTask;
        }
    }
}