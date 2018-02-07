namespace bgTeam.Infrastructure
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using RabbitMQ.Client;

    public class QueueWatcherRabbitMQ : IQueueWatcher<IQueueMessage>
    {
        private readonly IAppLogger _logger;
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

            _threadSleep = 30000;
        }

        public event QueueMessageHandler OnSubscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> OnError;

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

                    ////var taskId = Task.CurrentId;
                    ////_logger.Debug($"  Start read queue - {queueName}:{taskId}");

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

                        OnError?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp.GetBaseException()));

                        // TODO : Не ждём если возникла ошибка при обработке
                        // await Task.Delay(_threadSleep);
                    }
                    catch (Exception exp)
                    {
                        _logger.Fatal(exp);

                        // Ждём 5 сек
                        await Task.Delay(5000);
                    }
                    finally
                    {
                        ////_logger.Debug($"  End read queue - {queueName}:{taskId}");

                        _semaphore.Release();
                    }
                });
            }
        }

        protected async Task<bool> AskMessage(string queueName)
        {
            _logger.Debug($"QueueWatcherRabbitMQ: create connect to {queueName}");

            using (var connection = _factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                _logger.Debug($"QueueWatcherRabbitMQ: connect open");

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
                        throw new ProcessMessageException(message, "bgTeam: Exception during procesing queue message", exp);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
