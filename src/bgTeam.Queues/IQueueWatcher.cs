namespace bgTeam.Queues
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.Queues.Exceptions;

    public delegate Task QueueMessageReceive<TEntity>(object queue, TEntity message)
        where TEntity : IQueueMessage;

    public interface IQueueWatcher<TQueueMessage>
    {
        event QueueMessageHandler Subscribe;

        event EventHandler<ExtThreadExceptionEventArgs> Error;

        void StartWatch(string queueName);
    }
}