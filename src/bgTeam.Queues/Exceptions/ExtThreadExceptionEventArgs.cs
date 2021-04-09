namespace bgTeam.Queues.Exceptions
{
    using System;
    using System.Threading;
    using bgTeam.Queues;

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
