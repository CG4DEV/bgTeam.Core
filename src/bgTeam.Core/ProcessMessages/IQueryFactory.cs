namespace bgTeam.ProcessMessages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Queues;

    public interface IQueryFactory
    {
        string ForMessageType { get; }

        IQuery CreateQuery(IQueueMessage msg);

        Task<IQuery> CreateQueryAsync (IQueueMessage msg);
    }
}
