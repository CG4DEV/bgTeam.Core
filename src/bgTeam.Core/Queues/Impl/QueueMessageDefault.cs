namespace bgTeam.Queues
{
    using System.Collections.Generic;

    public class QueueMessageDefault : IQueueMessage
    {
        public QueueMessageDefault(string body)
        {
            Body = body;
        }

        public int? Delay { get; set; }

        public IList<string> Errors { get; set; }

        public string Body { get; set; }
    }
}
