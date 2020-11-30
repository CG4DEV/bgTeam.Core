using System;
using System.Collections.Generic;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Xunit;

namespace bgTeam.Core.Tests.Rabbit
{
    public class ConnectionFactoryRabbitMQTests
    {
        [Fact]
        public void DependencyQueueProviderSettings()
        {
            var queueProviderSettings = GetMocks();
            Assert.Throws<ArgumentNullException>("settings", () =>
            {
                new ConnectionFactoryRabbitMQ(null);
            });
        }

        [Fact]
        public void CreateConnectionShouldThrowsExceptionIfUsedNotSuitableHost()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(new QueueProviderSettings
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
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            var dict = new Dictionary<string, object>();
            dict.Add("prop1", "value");
            connectionFactoryRabbitMQ.ClientProperties = dict;
            Assert.True(connectionFactoryRabbitMQ.ClientProperties.TryGetValue("prop1", out object value));
            Assert.Equal("value", (string)value);
        }

        [Fact]
        public void PasswordGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Password = "pass";
            Assert.Equal("pass", connectionFactoryRabbitMQ.Password);
        }

        [Fact]
        public void RequestedChannelMaxGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedChannelMax = 120;
            Assert.Equal(120, connectionFactoryRabbitMQ.RequestedChannelMax);
        }

        [Fact]
        public void RequestedFrameMaxGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedFrameMax = 120;
            Assert.Equal((uint)120, connectionFactoryRabbitMQ.RequestedFrameMax);
        }

        [Fact]
        public void RequestedHeartbeatGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.RequestedHeartbeat = TimeSpan.FromSeconds(1);
            Assert.Equal(TimeSpan.FromSeconds(1), connectionFactoryRabbitMQ.RequestedHeartbeat);
        }

        [Fact]
        public void UseBackgroundThreadsForIOGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.UseBackgroundThreadsForIO = true;
            Assert.True(connectionFactoryRabbitMQ.UseBackgroundThreadsForIO);
        }

        [Fact]
        public void UserNameGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.UserName = "u";
            Assert.Equal("u", connectionFactoryRabbitMQ.UserName);
        }

        [Fact]
        public void VirtualHostGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.VirtualHost = "h";
            Assert.Equal("h", connectionFactoryRabbitMQ.VirtualHost);
        }

        [Fact]
        public void HostNameGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.HostName = "host";
            Assert.Equal("host", connectionFactoryRabbitMQ.HostName);
        }

        [Fact]
        public void PortGetSet()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Port = 5333;
            Assert.Equal(5333, connectionFactoryRabbitMQ.Port);
        }

        [Fact]
        public void Uri()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.Uri = new Uri("amqp://test.ru");
            Assert.Equal("amqp://test.ru/", connectionFactoryRabbitMQ.Uri.ToString());
        }

        [Fact]
        public void HandshakeContinuationTimeout()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.HandshakeContinuationTimeout = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.HandshakeContinuationTimeout);
        }

        [Fact]
        public void ContinuationTimeout()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.ContinuationTimeout = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.ContinuationTimeout);
        }

        [Fact]
        public void NetworkRecoveryInterval()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.NetworkRecoveryInterval = new TimeSpan(5, 10, 15);
            Assert.Equal(new TimeSpan(5, 10, 15), connectionFactoryRabbitMQ.NetworkRecoveryInterval);
        }

        [Fact]
        public void AutomaticRecoveryEnabled()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.AutomaticRecoveryEnabled = true;
            Assert.True(connectionFactoryRabbitMQ.AutomaticRecoveryEnabled);
        }

        [Fact]
        public void AuthMechanismFactory()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            connectionFactoryRabbitMQ.AuthMechanismFactory(new List<string>() { });
        }

        [Fact]
        public void CreateConnectionWithClientProvidedName()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection("clientProvidedName"));
        }

        [Fact]
        public void CreateConnectionWithHostnames()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<string>() { }));
        }

        [Fact]
        public void CreateConnectionWithClientProvidedNameAndHostnames()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<string>() { }, "clientProvidedName"));
        }

        [Fact]
        public void CreateConnectionWithClientProvidedNameAndEndpoints()
        {
            var queueProviderSettings = GetMocks();
            var connectionFactoryRabbitMQ = new ConnectionFactoryRabbitMQ(queueProviderSettings.Object);
            Assert.Throws<NotImplementedException>(() => connectionFactoryRabbitMQ.CreateConnection(new List<AmqpTcpEndpoint>() { }));
        }

        private Mock<IQueueProviderSettings> GetMocks()
        {
            var queueProviderSettings = new Mock<IQueueProviderSettings>();

            return queueProviderSettings;
        }
    }
}
