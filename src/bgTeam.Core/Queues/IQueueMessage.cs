namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IQueueMessage
    {
        int? Delay { get; }

        IList<string> Errors { get; set; }

        string Body { get; }
    }

    public class QueueMessage : IQueueMessage
    {
        public int? Delay { get; set; }

        public IList<string> Errors { get; set; }

        public string Body { get; set; }

        public QueueMessage(string body)
        {
            Body = body;
        }
    }
}
