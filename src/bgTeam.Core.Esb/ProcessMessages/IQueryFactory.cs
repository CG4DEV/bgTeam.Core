namespace bgTeam.ProcessMessages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Queues;

    public interface IQueryFactory
    {
        string ForMessageType { get; }

        IEnumerable<IQuery> CreateQuery(IQueueMessage msg);

        Task<IEnumerable<IQuery>> CreateQueryAsync(IQueueMessage msg);
    }
}
