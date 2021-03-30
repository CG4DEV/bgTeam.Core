using System.Collections.Generic;
using bgTeam.Impl.Kafka;

namespace KafkaSingleThread
{
    class KafkaSettings : IKafkaSettings
    {
        public IDictionary<string, string> Config { get; set; }
    }
}
