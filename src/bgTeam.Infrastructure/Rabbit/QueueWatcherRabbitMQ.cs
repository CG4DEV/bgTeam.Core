namespace bgTeam.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Exceptions;
    using bgTeam.Exceptions.Args;
    using bgTeam.Queues;
    using RabbitMQ.Client;

    public class QueueWatcherRabbitMQ : IQueueWatcher<IQueueMessage>
    {
        public event QueueMessageHandler OnSubscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> OnError;

        protected readonly IAppLogger _logger;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        private readonly int _threadSleep;
        private readonly SemaphoreSlim _semaphore;

        public QueueWatcherRabbitMQ(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IQueueProviderSettings settings,
            int threadsCount = 1)
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

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);

#if DEBUG
            _threadSleep = 5000;
#else
            _threadSleep = 5000;
#endif
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
                    var noMsg = false;
                    var taskId = Task.CurrentId;

                    //_logger.Debug($"  Start read queue - {queueName}:{taskId}");

                    try
                    {
                        noMsg = await AskMessage(queueName);

                        if (!noMsg)
                        {
                            _logger.Info("No messages");
                            await Task.Delay(_threadSleep);
                        }
                    }
                    catch (ProcessMessageException exp)
                    {
                        _logger.Error(exp);

                        OnError?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp));

                        // TODO : Не ждём если возникла ошибка при обработке
                        // await Task.Delay(_threadSleep);
                    }
                    catch (Exception exp)
                    {
                        _logger.Error(exp);

                        await Task.Delay(_threadSleep);
                    }
                    finally
                    {
                        //_logger.Debug($"  End read queue - {queueName}:{taskId}");

                        _semaphore.Release();
                    }
                });
            }
        }

        protected async Task<bool> AskMessage(string queueName)
        {
            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var res = channel.BasicGet(queueName, false);
                if (res != null)
                {
                    var message = _msgProvider.ExtractObject(res.Body);
                    Exception exp = null;
                    try
                    {
                        await OnSubscribe(message);
                    }
                    catch (Exception ex)
                    {
                        exp = ex;
                    }
                    finally
                    {
                        channel.BasicAck(res.DeliveryTag, false);
                    }

                    if (exp != null)
                    {
                        throw new ProcessMessageException(message, "Exception during procesing queue message", exp);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
