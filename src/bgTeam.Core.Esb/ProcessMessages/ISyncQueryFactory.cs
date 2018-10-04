namespace bgTeam.ProcessMessages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.Queues;

    public interface ISyncQueryFactory
    {
        string ForMessageType { get; }

        IEnumerable<ISyncQuery> CreateQuery(IQueueMessage msg);

        Task<IEnumerable<ISyncQuery>> CreateQueryAsync(IQueueMessage msg);
    }
}
