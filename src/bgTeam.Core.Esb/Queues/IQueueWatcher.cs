namespace bgTeam.Queues
{
    using bgTeam.Exceptions.Args;
    using System;
    using System.Threading.Tasks;

    public delegate Task QueueMessageReceive<TEntity>(object queue, TEntity message)
        where TEntity : IQueueMessage;

    public interface IQueueWatcher<IQueueMessage>
    {
        event QueueMessageHandler OnSubscribe;

        event EventHandler<ExtThreadExceptionEventArgs> OnError;

        void StartWatch(string queueName);
    }
}