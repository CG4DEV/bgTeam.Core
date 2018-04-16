namespace bgTeam.Queues
{
    public interface ISenderEntity
    {
        void Send<T>(IQueueMessage msg, params string[] queues);

        //void Send<T>(object entity, string entityType)
        //    where T : IQueueMessage;

        //void Send<T>(object entity, string entityType, int? delay)
        //    where T : IQueueMessage;

        void Send<T>(object entity, string entityType, params string[] queues)
            where T : IQueueMessage;

        void Send<T>(object entity, string entityType, int? delay, params string[] queues)
            where T : IQueueMessage;
    }
}
