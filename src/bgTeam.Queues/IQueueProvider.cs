namespace bgTeam.Queues
{
    using System.Threading.Tasks;

    public delegate Task QueueMessageHandler(IQueueMessage message);

    public interface IQueueProvider
    {
        void PushMessage(IQueueMessage message);

        void PushMessage(IQueueMessage message, params string[] queues);

        QueueMessageWork AskMessage(string queueName);

        void DeleteMessage(QueueMessageWork message);

        uint GetQueueMessageCount(string queueName);
    }
}