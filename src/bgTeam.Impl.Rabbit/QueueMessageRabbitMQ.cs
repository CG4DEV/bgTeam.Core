namespace bgTeam.Impl.Rabbit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using bgTeam.Queues;

    public class QueueMessageRabbitMQ : IQueueMessage
    {
        private const int DelayStep = 900000;

        public QueueMessageRabbitMQ()
        {
            Errors = new List<string>();
        }

        public QueueMessageRabbitMQ(string body)
        {
            Body = body;
            Errors = new List<string>();
        }

        public Guid Uid { get; set; }

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
            set
            {
            }
        }

        public IList<string> Errors { get; set; }
    }
}
