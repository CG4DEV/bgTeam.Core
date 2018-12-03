namespace bgTeam.Queues
{
    using System;

    public interface ISenderEntity : IDisposable
    {
        void Send(IQueueMessage msg, params string[] queues);

        void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage, new();

        void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage, new();
    }
}
