namespace bgTeam.Impl.Kafka
{
    using bgTeam.Queues;

    public interface IKafkaMessage : IQueueMessage
    {
        string Key { get; set; }

        int Partition { get; set; }

        long Offset { get; set; }
    }
}
