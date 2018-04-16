using bgTeam.Impl;
using bgTeam.Impl.Rabbit;
using bgTeam.Queues;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Test.bgTeam.Impl.Rabbit.Common;
using Xunit;

namespace Test.bgTeam.Impl.Rabbit.Tests
{
    public class Test_QueueWatcherRabbitMQ
    {
        [Fact(Skip = "Test Exception Message")]
        public void Test_QueueWatcherRabbitMQ_Test1()
        {
            var logger = new AppLoggerDefault();
            var msgprovider = new MessageProviderDefault();

            var fmock = new Mock<IConnectionFactory>();
            fmock.Setup(f => f.CreateConnection()).Returns(new TestConnection());

            var watch = new QueueWatcherRabbitMQ(logger, msgprovider, fmock.Object);

            watch.OnSubscribe += Watch_OnSubscribe;
            watch.OnError += Watch_OnError;

            watch.StartWatch("Test");
        }

        private Task Watch_OnSubscribe(IQueueMessage message)
        {
            throw new ArgumentNullException("test");
        }

        private void Watch_OnError(object sender, global::bgTeam.Queues.Exceptions.ExtThreadExceptionEventArgs e)
        {
            Assert.IsType(typeof(ArgumentNullException), e.Exception);
            Assert.Equal("test", e.Exception.Message);
        }
    }
}
