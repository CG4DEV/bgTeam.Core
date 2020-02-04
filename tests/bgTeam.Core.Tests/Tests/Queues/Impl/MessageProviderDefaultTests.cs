using bgTeam.Queues;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Queues.Impl
{
    public class MessageProviderDefaultTests
    {
        [Fact]
        public void PrepareMessageStr()
        {
            var provider = new MessageProviderDefault();
            var msg = provider.PrepareMessageStr(new Test());
            Assert.Equal("{\"$type\":\"bgTeam.Core.Tests.Tests.Queues.Impl.MessageProviderDefaultTests+Test, bgTeam.Core.Tests\",\"Name\":\"John\"}", msg);
        }

        [Fact]
        public void PrepareMessageByte()
        {
            var provider = new MessageProviderDefault();
            var bytes = provider.PrepareMessageByte(new Test());
            Assert.Equal("{\"$type\":\"bgTeam.Core.Tests.Tests.Queues.Impl.MessageProviderDefaultTests+Test, bgTeam.Core.Tests\",\"Name\":\"John\"}", Encoding.UTF8.GetString(bytes));
        }

        [Fact]
        public void ExtractObject()
        {
            var provider = new MessageProviderDefault();
            var message = provider.ExtractObject(JsonConvert.SerializeObject(new QueueMessageDefault { Body = "Hi" }));
            Assert.Equal("Hi", message.Body);
        }

        [Fact]
        public void ExtractObjectBytes()
        {
            var provider = new MessageProviderDefault();
            var message = provider.ExtractObject(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new QueueMessageDefault { Body = "Hi" })));
            Assert.Equal("Hi", message.Body);
        }

        private class Test
        {
            public string Name = "John";
        }
    }
}
