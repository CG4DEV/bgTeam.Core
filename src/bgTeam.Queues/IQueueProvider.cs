namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public delegate Task QueueMessageHandler(IQueueMessage message);

    public interface IQueueProvider : IDisposable
    {
        void PushMessage(IQueueMessage message);

        void PushMessage(IQueueMessage message, params string[] queues);

        void PushMessages(IEnumerable<IQueueMessage> messages);

        void PushMessages(IEnumerable<IQueueMessage> messages, params string[] queues);

        //QueueMessageWork AskMessage(string queueName);

        //void DeleteMessage(QueueMessageWork message);

        uint GetQueueMessageCount(string queueName);
    }
}