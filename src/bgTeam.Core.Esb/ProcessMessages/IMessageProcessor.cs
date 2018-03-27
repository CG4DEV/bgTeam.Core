namespace bgTeam.ProcessMessages
{
    using bgTeam.Queues;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessageProcessor
    {
        IEnumerable<ISyncQuery> Process(IQueueMessage message);

        Task<IEnumerable<ISyncQuery>> ProcessAsync(IQueueMessage message);
    }
}
