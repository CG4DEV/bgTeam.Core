namespace bgTeam.Infrastructure.Rabbit
{
    using System.Collections.Generic;
    using bgTeam.Queues;

    public class QueueMessageRabbitMQ : IQueueMessage
    {
        private const int DelayStep = 900000;

        public QueueMessageRabbitMQ(string body)
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

                return (1 << Errors.Count) * DelayStep;
            }
        }
    }
}
