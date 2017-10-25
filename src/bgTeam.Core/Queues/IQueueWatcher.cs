namespace bgTeam.Queues
{
    using System.Threading.Tasks;

    public delegate Task QueueMessageReceive<TEntity>(object queue, TEntity message)
        where TEntity : IQueueMessage;

    public interface IQueueWatcher<IQueueMessage>
    {
        event QueueMessageHandler OnSubscribe;

        void StartWatch(string queueName);

        //event EventHandler<ExtThreadExceptionEventArgs> OnError;
    }
}