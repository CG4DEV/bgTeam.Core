namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;

    public class QueueMessageDefault : IQueueMessage
    {
        public QueueMessageDefault()
        {
            Errors = new List<string>();
        }

        public QueueMessageDefault(string body)
        {
            Body = body;
            Errors = new List<string>();
        }

        public Guid Uid { get; set; }

        public string Body { get; set; }

        public int? Delay { get; set; }

        public IList<string> Errors { get; set; }
    }
}
