namespace bgTeam.ProcessMessages
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Queues;

    public interface ISyncQueryFactory
    {
        string ForMessageType { get; }

        IEnumerable<ISyncQuery> CreateQuery(IQueueMessage msg, IDbConnection connection = null);

        Task<IEnumerable<ISyncQuery>> CreateQueryAsync(IQueueMessage msg, IDbConnection connection = null);

        IDbConnection CreateConnection();

        Task<IDbConnection> CreateConnectionAsync();
    }
}
