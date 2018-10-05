namespace bgTeam.ProcessMessages
{
    using bgTeam.Queues;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public interface IMessageProcessor
    {
        IEnumerable<ISyncQuery> Process(IQueueMessage message, IDbConnection connection = null);

        Task<IEnumerable<ISyncQuery>> ProcessAsync(IQueueMessage message, IDbConnection connection = null);

        IDbConnection CreateConnection(IQueueMessage message);

        Task<IDbConnection> CreateConnectionAsync(IQueueMessage message);
    }
}
