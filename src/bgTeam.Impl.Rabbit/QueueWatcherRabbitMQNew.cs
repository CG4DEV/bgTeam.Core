namespace bgTeam.Impl.Rabbit
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using RabbitMQ.Client;

    public class QueueWatcherRabbitMQNew : IQueueWatcher<IQueueMessage>
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        private readonly int _threadSleep;
        private readonly SemaphoreSlim _semaphore;

        public QueueWatcherRabbitMQNew(
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

        public QueueWatcherRabbitMQNew(
            IAppLogger logger,
            IMessageProvider msgProvider,
            IConnectionFactory factory,
            int threadsCount = 1)
        {
            _logger = logger;
            _msgProvider = msgProvider;
            _factory = factory;

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);

            _threadSleep = 30000;
        }

        public event QueueMessageHandler OnSubscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> OnError;

        public event EventHandler<ExtThreadExceptionEventArgs> OnWarning;

        public void StartWatch(string queueName)
        {
            if (OnSubscribe == null)
            {
                throw new ArgumentNullException(nameof(OnSubscribe));
            }

            while (true)
            {
                _logger.Debug($"NewQueueWatcherRabbitMQ: create connection");
                using (var connection = _factory.CreateConnection())
                {
                    try
                    {
                        MainLoop(queueName, connection);
                    }
                    catch (Exception ex)
                    {
                        var args = connection.CloseReason;
                        if (args != null)
                        {
                            _logger.Error($"NewQueueWatcherRabbitMQ: connection shutdown. Reason: {args.ReplyCode} - {args.ReplyText}");
                        }
                        else
                        {
                            _logger.Error($"NewQueueWatcherRabbitMQ: error: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void MainLoop(string queueName, IConnection connection)
        {
            while (true)
            {
                _semaphore.Wait();
                Task.Factory.StartNew(async () =>
                {
                    var noMsg = false;

                    try
                    {
                        noMsg = await AskMessage(queueName, connection);

                        if (!noMsg)
                        {
                            _logger.Info("No messages");
                            await Task.Delay(_threadSleep);
                        }
                    }
                    catch (ProcessMessageException exp) when (exp.InnerException is BgTeamException bexp)
                    {
                        _logger.Warning($"Exception of type {bexp.GetType().Name}: {bexp.Message}{Environment.NewLine}{bexp.StackTrace}");

                        OnWarning?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, bexp));
                    }
                    catch (ProcessMessageException exp)
                    {
                        _logger.Error(exp);

                        OnError?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp.GetBaseException()));
                    }
                    catch (Exception exp)
                    {
                        _logger.Fatal(exp);

                        // Ждём 5 сек
                        await Task.Delay(5000);
                    }
                    finally
                    {
                        _semaphore.Release();
                    }
                });
            }
        }

        protected async Task<bool> AskMessage(string queueName, IConnection connection)
        {
            using (var channel = connection.CreateModel())
            {
                _logger.Debug($"NewQueueWatcherRabbitMQ: channel open");
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
                        throw new ProcessMessageException(message, $"bgTeam: {exp.Message}", exp);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
