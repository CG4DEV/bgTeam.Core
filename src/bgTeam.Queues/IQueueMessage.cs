namespace bgTeam.Queues
{
    using System.Collections.Generic;

    public interface IQueueMessage
    {
        string Body { get; set; }

        int? Delay { get; set; }

        IList<string> Errors { get; set; }
    }
}
