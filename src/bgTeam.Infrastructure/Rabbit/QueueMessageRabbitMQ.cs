namespace bgTeam.Infrastructure.Rabbit
{
    using System.Collections.Generic;
    using bgTeam.Queues;
    using System.Linq;

    public class QueueMessageRabbitMQ : IQueueMessage
    {
        private const int DelayStep = 900000;

        public QueueMessageRabbitMQ(string body)
        {
            Body = body;
            Errors = new List<string>();
        }

        public IList<string> Errors { get; set; }

        public string Body { get; set; }

        public int? Delay
        {
            get
            {
                if (Errors == null || !Errors.Any())
                {
                    return 0;
                }

                return (1 << Errors.Count) * DelayStep;
            }
        }
    }
}
