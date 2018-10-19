namespace bgTeam.Queues
{
    using bgTeam.Queues.Exceptions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueueWatcherDefault : IQueueWatcher<IQueueMessage>
    {
        private readonly int _threadSleep = 30000;

        private readonly IAppLogger _logger;
        private readonly IQueueProvider _queueProvider;
        private readonly IMessageProvider _messageProvider;
        private readonly SemaphoreSlim _semaphore;

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

        public event QueueMessageHandler OnSubscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> OnError;


        protected IAppLogger Logger => _logger;

        public void StartWatch(string queueName)
        {
            if (OnSubscribe == null)
            {
                throw new ArgumentNullException(nameof(OnSubscribe));
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
                    catch (ProcessMessageException exp) when (exp.InnerException is BgTeamException bexp)
                    {
                        _logger.Warning($"Exception of type {bexp.GetType().Name}: {bexp.Message}{Environment.NewLine}{bexp.StackTrace}");

                        OnError?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, bexp));
                    }
                    catch (ProcessMessageException exp)
                    {
                        _logger.Error(exp);
                        OnError?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp));
                    }
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
}