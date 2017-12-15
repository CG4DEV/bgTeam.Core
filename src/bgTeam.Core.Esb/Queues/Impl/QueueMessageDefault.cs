namespace bgTeam.Queues
{
    using System.Collections.Generic;

    public class QueueMessageDefault : IQueueMessage
    {
        private const int DelayStep = 20;

        public QueueMessageDefault(string body)
        {
            Body = body;
        }

        public IList<string> Errors { get; set; }

        public string Body { get; set; }

        public int? Delay
        {
            get
            {
                if (Errors.Count == 0)
                {
                    return 0;
                }

                return (1 << (Errors.Count - 1)) * DelayStep;
            }
        }
    }
}
