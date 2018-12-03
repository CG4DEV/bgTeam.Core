
namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;

    public interface ISenderEntities : IDisposable
    {
        void SendList<T>(IEnumerable<object> entities, string entityType, int? delay = null, params string[] queues)
            where T : IQueueMessage;
    }
}
