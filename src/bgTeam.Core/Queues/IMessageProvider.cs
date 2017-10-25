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
}