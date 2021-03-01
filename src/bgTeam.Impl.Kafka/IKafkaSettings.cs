namespace bgTeam.Impl.Kafka
{
    using System.Collections.Generic;

    public interface IKafkaSettings
    {
        IDictionary<string, string> Config { get; }
    }
}
