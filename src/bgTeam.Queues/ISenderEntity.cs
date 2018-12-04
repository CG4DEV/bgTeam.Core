namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;

    public interface ISenderEntity : IDisposable
    {
        void Send(IQueueMessage msg, params string[] queues);

        void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage, new();

        void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage, new();

        void SendList<T>(IEnumerable<object> entities, string entityType, int? delay = null, params string[] queues)
            where T : IQueueMessage;
    }
}
