namespace bgTeam.Queues
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class QueueWatcherWarningException : Exception
    {
        public QueueWatcherWarningException()
        {
        }

        public QueueWatcherWarningException(string message)
            : base(message)
        {
        }

        public QueueWatcherWarningException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected QueueWatcherWarningException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
