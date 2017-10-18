namespace bgTeam.Queues
{
    using Newtonsoft.Json;
    using System.Text;

    public interface IMessageProvider
    {
        string PrepareMessageStr(object source);

        byte[] PrepareMessageByte(object source);

        IQueueMessage ExtractObject(string source);

        IQueueMessage ExtractObject(byte[] source);
    }

    public class DefaultMessageProvider : IMessageProvider
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
            return JsonConvert.DeserializeObject<QueueMessage>(source);
        }

        public IQueueMessage ExtractObject(byte[] source)
        {
            var str = Encoding.UTF8.GetString(source);

            return JsonConvert.DeserializeObject<QueueMessage>(str);
        }
    }
}