namespace bgTeam.Queues
{
    using bgTeam.Exceptions.Args;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueueWatcher<IQueueMessage>
    {
        event QueueMessageHandler OnSubscribe;

        event EventHandler<ExtThreadExceptionEventArgs> OnError;

        void StartWatch(string queueName);
    }

    public class QueueWatcherDefault : IQueueWatcher<IQueueMessage>
    {
        protected readonly IAppLogger _logger;
        private readonly IQueueProvider _queueProvider;
        private readonly IMessageProvider _messageProvider;
        private readonly SemaphoreSlim _semaphore;

#if DEBUG
        private readonly int _threadSleep = 5000;
#else
        private readonly int _threadSleep = 30000;
#endif

        public event QueueMessageHandler OnSubscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> OnError;

        public QueueWatcherDefault(
            IAppLogger logger,
            IQueueProvider queueProvider,
            IMessageProvider messageProvider,
            int threadsCount = 1)
        {
            _logger = logger;
            _queueProvider = queueProvider;
            _messageProvider = messageProvider;

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);
        }

        public void StartWatch(string queueName)
        {
            if (OnSubscribe == null)
            {
                throw new ArgumentNullException("OnSubscribe");
            }

            while (true)
            {
                _semaphore.Wait();
                Task.Factory.StartNew(async () =>
                {
                    _logger.Debug($"Start task - {Task.CurrentId}");

                    try
                    {
                        await DispatchRoutine(queueName);
                    }
                    //catch (DeserializeException exception)
                    //{
                    //    Logger.Error(exception.InnerException);
                    //    DeleteMessageFromQueueWitoutException(queueName, exception.MessageId, exception.MessageReservationId);
                    //}
                    catch (Exception exp)
                    {
                        _logger.Error(exp);
                    }
                    finally
                    {
                        _logger.Debug($"End task - {Task.CurrentId}");
                        ////await Task.Delay(2000);
                        _semaphore.Release();
                    }
                });
            }
        }

        protected async Task DispatchRoutine(string queueName)
        {
            var message = _queueProvider.AskMessage(queueName);
            if (message != null)
            {
                var entity = _messageProvider.ExtractObject(message.Body);

                await OnSubscribe(entity);

                _queueProvider.DeleteMessage(message);
            }
            else
            {
                _logger.Info("No messages");
                await Task.Delay(_threadSleep);
            }
        }
    }

    public delegate Task QueueMessageReceive<TEntity>(object queue, TEntity message)
        where TEntity : IQueueMessage;
}