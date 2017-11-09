namespace bgTeam.ProcessMessages
{
    using bgTeam.Queues;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMessageProcessor
    {
        IEnumerable<IQuery> Process(IQueueMessage message);

        Task<IEnumerable<IQuery>> ProcessAsync(IQueueMessage message);
    }
}
