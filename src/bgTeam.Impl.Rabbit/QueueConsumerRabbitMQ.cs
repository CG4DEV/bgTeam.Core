namespace bgTeam.Impl.Rabbit
{
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;
    using System;
    using System.Text;

    public class QueueConsumerRabbitMQ : BaseQueueConsumerRabbitMQ<EventingBasicConsumer>, IQueueWatcher<IQueueMessage>
    {
        public QueueConsumerRabbitMQ(IConnectionFactory connectionFactory, IMessageProvider provider)
            : this(connectionFactory, provider, PREFETCHCOUNT)
        {
        }

        public QueueConsumerRabbitMQ(
            IConnectionFactory connectionFactory,
            IMessageProvider provider,
            ushort prefetchCount)
            : base(connectionFactory, provider, prefetchCount)
        {
        }

        private void ReceiverHandler(object sender, BasicDeliverEventArgs e)
        {
            var message = _provider.ExtractObject(Encoding.UTF8.GetString(e.Body.ToArray()));
            try
            {
                OnSubscribe(message).Wait();
            }
            catch (Exception ex)
            {
                OnError(this, new ExtThreadExceptionEventArgs(message, ex));
            }
            finally
            {
                var model = ((EventingBasicConsumer)sender).Model;
                model.BasicAck(e.DeliveryTag, false);
            }
        }

        private void ShutdownHandler(object sender, ShutdownEventArgs e)
        {
            InitConsumer();
            StartWatch(_watchingQueueName);
        }

        protected override void InitConsumer()
        {
            if (_consumer != null)
            {
                _consumer.Received -= ReceiverHandler;
                _consumer.Shutdown -= ShutdownHandler;
            }

            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += ReceiverHandler;
            _consumer.Shutdown += ShutdownHandler;
        }
    }
}