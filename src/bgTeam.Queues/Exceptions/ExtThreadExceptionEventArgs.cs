namespace bgTeam.Queues.Exceptions
{
    using bgTeam.Queues;
    using System;
    using System.Threading;

    public class ExtThreadExceptionEventArgs : ThreadExceptionEventArgs
    {
        public ExtThreadExceptionEventArgs(IQueueMessage message, Exception exp)
            : base(exp)
        {
            Message = message;
        }

        public IQueueMessage Message { get; private set; }
    }
}
