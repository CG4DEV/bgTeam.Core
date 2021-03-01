using System.Collections.Generic;
using bgTeam.Impl.Kafka;

namespace KafkaMultiThread
{
    class MultithreadKafkaSettings : IMultithreadKafkaSettings
    {
        public int ThreadCount { get; set; }
        public int BoundedCapacity { get; set; }
        public int CommitIntervalMs { get; set; }
        public IDictionary<string, string> Config { get; set; }
    }
}
