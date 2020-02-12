namespace bgTeam.Impl.Rabbit
{
    using bgTeam.Queues;
    using RabbitMQ.Client;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Фабрика подключений к RabbitMQ
    /// для удерживания подключения в singleton
    /// после переноса в базовые сборки отключить из Trcont.Common nuget библиотеку bgTeam.Impl.Rabbit
    /// </summary>
    public class ConnectionFactoryRabbitMQ : IConnectionFactory, IDisposable
    {
        private bool disposed = false;
        private IAppLogger _logger;
        private ConnectionFactory _connectionFactory;
        private readonly object _lock = new object();
        private IConnection _connection;

        // Maybe pass IConnection Factory in constructor? - Can't test this
        public ConnectionFactoryRabbitMQ(IAppLogger logger, IQueueProviderSettings settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            _connectionFactory = new ConnectionFactory()
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = settings.VirtualHost,
                UserName = settings.Login,
                Password = settings.Password,
            };
        }

        ~ConnectionFactoryRabbitMQ()
        {
            Dispose(false);
        }

        public IDictionary<string, object> ClientProperties
        {
            get { return _connectionFactory.ClientProperties; }
            set { _connectionFactory.ClientProperties = value; }
        }

        public string Password
        {
            get { return _connectionFactory.Password; }
            set { _connectionFactory.Password = value; }
        }

        public ushort RequestedChannelMax
        {
            get { return _connectionFactory.RequestedChannelMax; }
            set { _connectionFactory.RequestedChannelMax = value; }
        }

        public uint RequestedFrameMax
        {
            get { return _connectionFactory.RequestedFrameMax; }
            set { _connectionFactory.RequestedFrameMax = value; }
        }

        public ushort RequestedHeartbeat
        {
            get { return _connectionFactory.RequestedHeartbeat; }
            set { _connectionFactory.RequestedHeartbeat = value; }
        }

        public bool UseBackgroundThreadsForIO
        {
            get { return _connectionFactory.UseBackgroundThreadsForIO; }
            set { _connectionFactory.UseBackgroundThreadsForIO = value; }
        }

        public string UserName
        {
            get { return _connectionFactory.UserName; }
            set { _connectionFactory.UserName = value; }
        }

        public string VirtualHost
        {
            get { return _connectionFactory.VirtualHost; }
            set { _connectionFactory.VirtualHost = value; }
        }

        public string HostName
        {
            get { return _connectionFactory.HostName; }
            set { _connectionFactory.HostName = value; }
        }

        public int Port
        {
            get { return _connectionFactory.Port; }
            set { _connectionFactory.Port = value; }
        }

        public Uri Uri
        {
            get { return _connectionFactory.Uri; }
            set { _connectionFactory.Uri = value; }
        }

        [Obsolete]
        public TaskScheduler TaskScheduler
        {
            get { return _connectionFactory.TaskScheduler; }
            set { _connectionFactory.TaskScheduler = value; }
        }

        public TimeSpan HandshakeContinuationTimeout
        {
            get { return _connectionFactory.HandshakeContinuationTimeout; }
            set { _connectionFactory.HandshakeContinuationTimeout = value; }
        }

        public TimeSpan ContinuationTimeout
        {
            get { return _connectionFactory.ContinuationTimeout; }
            set { _connectionFactory.ContinuationTimeout = value; }
        }

        public TimeSpan NetworkRecoveryInterval
        {
            get { return _connectionFactory.NetworkRecoveryInterval; }
            set { _connectionFactory.NetworkRecoveryInterval = value; }
        }

        public bool AutomaticRecoveryEnabled
        {
            get { return _connectionFactory.AutomaticRecoveryEnabled; }
            set { _connectionFactory.AutomaticRecoveryEnabled = value; }
        }

        public AuthMechanismFactory AuthMechanismFactory(IList<string> mechanismNames)
        {
            return _connectionFactory.AuthMechanismFactory(mechanismNames);
        }

        public IConnection CreateConnection()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                lock (_lock)
                {
                    if (_connection == null || !_connection.IsOpen)
                    {
                        _connection = _connectionFactory.CreateConnection();
                    }
                }
            }

            return _connection;
        }

        public IConnection CreateConnection(string clientProvidedName)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<string> hostnames)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<string> hostnames, string clientProvidedName)
        {
            throw new NotImplementedException();
        }

        public IConnection CreateConnection(IList<AmqpTcpEndpoint> endpoints)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);

            // подавляем финализацию
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing && _connection != null)
                {
                    // Освобождаем управляемые ресурсы
                    _connection.Close();
                    _connection.Dispose();
                }

                // освобождаем неуправляемые объекты
                _connection = null;
                _logger = null;
                _connectionFactory = null;
                disposed = true;
            }
        }
    }
}
