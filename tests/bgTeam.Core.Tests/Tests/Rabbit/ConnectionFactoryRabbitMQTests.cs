using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class ConnectionFactoryRabbitMQTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new ConnectionFactoryRabbitMQ(null, queueProviderSettings.Object);
            });
        }

        [Fact]
        public void DependencyQueueProviderSettings()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            Assert.Throws<ArgumentNullException>("settings", () =>
            {
                new ConnectionFactoryRabbitMQ(appLogger.Object, null);
            });
        }

        [Fact]
        public async Task CreateConnectionShouldThrowsExceptionIfUsedNotSuitableHost()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, new QueueProviderSettings
            {
                Host = "localhost",
                Login = "guest",
                Password = "guest",
                VirtualHost = "virtualHost",
            });

            Assert.Throws<BrokerUnreachableException>(() => connectionFactoryRabbitMQ.CreateConnection());
        }

        [Fact]
        public void ClientPropertiesGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            var dict = new Dictionary<string, object>();
            dict.Add("prop1", "value");
            connectionFactoryRabbitMQ.ClientProperties = dict;
            Assert.True(connectionFactoryRabbitMQ.ClientProperties.TryGetValue("prop1", out object value));
            Assert.Equal("value", (string)value);
        }

        [Fact]
        public void PasswordGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Password = "pass";
            Assert.Equal("pass", connectionFactoryRabbitMQ.Password);
        }

        [Fact]
        public void RequestedChannelMaxGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedChannelMax = 120;
            Assert.Equal(120, connectionFactoryRabbitMQ.RequestedChannelMax);
        }

        [Fact]
        public void RequestedFrameMaxGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedFrameMax = 120;
            Assert.Equal((uint)120, connectionFactoryRabbitMQ.RequestedFrameMax);
        }

        [Fact]
        public void RequestedHeartbeatGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedHeartbeat = 120;
            Assert.Equal(120, connectionFactoryRabbitMQ.RequestedHeartbeat);
        }

        [Fact]
        public void UseBackgroundThreadsForIOGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.UseBackgroundThreadsForIO = true;
            Assert.True(connectionFactoryRabbitMQ.UseBackgroundThreadsForIO);
        }

        [Fact]
        public void UserNameGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.UserName = "u";
            Assert.Equal("u", connectionFactoryRabbitMQ.UserName);
        }

        [Fact]
        public void VirtualHostGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.VirtualHost = "h";
            Assert.Equal("h", connectionFactoryRabbitMQ.VirtualHost);
        }

        [Fact]
        public void HostNameGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.HostName = "host";
            Assert.Equal("host", connectionFactoryRabbitMQ.HostName);
        }

        [Fact]
        public void PortGetSet()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Port = 5333;
            Assert.Equal(5333, connectionFactoryRabbitMQ.Port);
        }

        [Fact]
        public void Uri()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Uri = new Uri("amqp://test.ru");
            Assert.Equal("amqp://test.ru/", connectionFactoryRabbitMQ.Uri.ToString());
        }

        [Fact]
        public void HandshakeContinuationTimeout()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.HandshakeContinuationTimeout = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.HandshakeContinuationTimeout);
        }

        [Fact]
        public void ContinuationTimeout()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.ContinuationTimeout = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.ContinuationTimeout);
        }

        [Fact]
        public void NetworkRecoveryInterval()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.NetworkRecoveryInterval = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.NetworkRecoveryInterval);
        }

        [Fact]
        public void AutomaticRecoveryEnabled()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.AutomaticRecoveryEnabled = true;
            Assert.True(connectionFactoryRabbitMQ.AutomaticRecoveryEnabled);
        }

        [Fact]
        public void AuthMechanismFactory()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            connectionFactoryRabbitMQ.AuthMechanismFactory(new List<string>() { });
        }

        [Fact]
        public void CreateConnectionWithClientProvidedName()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection("clientProvidedName"));
        }

        [Fact]
        public void CreateConnectionWithHostnames()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<string>() { }));
        }

        [Fact]
        public void CreateConnectionWithClientProvidedNameAndHostnames()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<string>() { }, "clientProvidedName"));
        }

        [Fact]
        public void CreateConnectionWithClientProvidedNameAndEndpoints()
        {
            var (appLogger, queueProviderSettings) = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(appLogger.Object, queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<AmqpTcpEndpoint>() { }));
        }

        private (
            Mock<IAppLogger>,
            Mock<IQueueProviderSettings>)
            GetMocks()
        {
            var appLogger = new Mock<IAppLogger>();
            var queueProviderSettings = new Mock<IQueueProviderSettings>();

            return (
                appLogger,
                queueProviderSettings);
        }
    }
}
