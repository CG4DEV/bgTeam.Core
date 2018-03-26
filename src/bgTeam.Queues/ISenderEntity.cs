namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public interface ISenderEntity
    {
        void Send<T>(object entity, string entityKey)
            where T : IQueueMessage;

        void Send<T>(object entity, string entityKey, int? delay)
            where T : IQueueMessage;

        void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage;

        void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage;
    }
}
