namespace bgTeam.Queues
{
    using System.Text;
    using Newtonsoft.Json;

    public class MessageProviderDefault : IMessageProvider
    {
        public string PrepareMessageStr(object source)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MaxDepth = 5,
                TypeNameHandling = TypeNameHandling.All,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return JsonConvert.SerializeObject(source, jsonSerializerSettings);
        }

        public byte[] PrepareMessageByte(object source)
        {
            var str = PrepareMessageStr(source);

            return Encoding.UTF8.GetBytes(str);
        }

        public IQueueMessage ExtractObject(string source)
        {
            return JsonConvert.DeserializeObject<QueueMessageDefault>(source);
        }

        public IQueueMessage ExtractObject(byte[] source)
        {
            var str = Encoding.UTF8.GetString(source);

            return JsonConvert.DeserializeObject<QueueMessageDefault>(str);
        }
    }
}