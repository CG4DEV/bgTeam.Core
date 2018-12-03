namespace bgTeam.Queues
{
    using System;
    using System.Collections.Generic;

    public interface IQueueMessage
    {
        Guid Uid { get; set; }

        string Body { get; set; }

        int? Delay { get; set; }

        IList<string> Errors { get; set; }
    }
}
