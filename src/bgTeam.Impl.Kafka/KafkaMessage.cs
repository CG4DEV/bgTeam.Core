namespace bgTeam.Impl.Kafka
{
    using System;
    using System.Collections.Generic;

    public class KafkaMessage : IKafkaMessage
    {
        public string Key { get; set; }

        public int Partition { get; set; }

        public long Offset { get; set; }

        public Guid Uid { get; set; }

        public int? Delay { get; set; }

        public IList<string> Errors { get; set; }

        public string Body { get; set; }
    }
}
