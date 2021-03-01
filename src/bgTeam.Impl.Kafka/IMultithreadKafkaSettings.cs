namespace bgTeam.Impl.Kafka
{
    public interface IMultithreadKafkaSettings : IKafkaSettings
    {
        int ThreadCount { get; set; }

        int BoundedCapacity { get; set; }

        int CommitIntervalMs { get; set; }
    }
}
