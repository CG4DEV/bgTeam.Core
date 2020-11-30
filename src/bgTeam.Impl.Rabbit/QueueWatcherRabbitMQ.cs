namespace bgTeam.Impl.Rabbit
{
    using System;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Queues.Exceptions;
    using RabbitMQ.Client;

    [Obsolete("Using class bgTeam.Impl.Rabbit.QueueConsumerAsyncRabbitMQ instead")]
    public class QueueWatcherRabbitMQ : IQueueWatcher<IQueueMessage>
    {
        private readonly IConnectionFactory _factory;
        private readonly IMessageProvider _msgProvider;

        private readonly int _threadSleep;
        private readonly SemaphoreSlim _semaphore;

        public QueueWatcherRabbitMQ(
            IMessageProvider msgProvider,
            IQueueProviderSettings settings,
            int threadsCount = 1)
        {
            _msgProvider = msgProvider ?? throw new ArgumentNullException(nameof(msgProvider));
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

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

        public QueueWatcherRabbitMQ(
            IMessageProvider msgProvider,
            IConnectionFactory factory,
            int threadsCount = 1)
        {
            _msgProvider = msgProvider ?? throw new ArgumentNullException(nameof(msgProvider));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));

            _semaphore = new SemaphoreSlim(threadsCount, threadsCount);

            _threadSleep = 30000;
        }

        public event QueueMessageHandler Subscribe;

        public event EventHandler<ExtThreadExceptionEventArgs> Error;

        public void StartWatch(string queueName)
        {
            if (Subscribe == null)
            {
                throw new ArgumentNullException(nameof(Subscribe));
            }

            while (true)
            {
                using (var connection = _factory.CreateConnection())
                {
                    MainLoop(queueName, connection);
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
                            await Task.Delay(_threadSleep);
                        }
                    }
                    catch (QueueWatcherException exp) when (exp.InnerException is QueueWatcherWarningException qexp)
                    {
                        Error?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, qexp));
                    }
                    catch (QueueWatcherException exp)
                    {
                        Error?.Invoke(this, new ExtThreadExceptionEventArgs(exp.QueueMessage, exp.GetBaseException()));
                    }
                    catch (Exception exp)
                    {
                        Error?.Invoke(this, new ExtThreadExceptionEventArgs(null, exp));
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
                var res = channel.BasicGet(queueName, false);
                if (res != null)
                {
                    var message = _msgProvider.ExtractObject(Encoding.UTF8.GetString(res.Body.ToArray()));
                    Exception exp = null;
                    try
                    {
                        await Subscribe(message);
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
                        throw new QueueWatcherException($"bgTeam: {exp.Message}", message, exp);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
