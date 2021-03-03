namespace bgTeam.Queues.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class QueueWatcherException : Exception
    {
        public QueueWatcherException(string message, IQueueMessage queueMessage, Exception innerExeception)
            : base(message, innerExeception)
        {
            QueueMessage = queueMessage;
            Source = queueMessage.Body;
        }

        protected QueueWatcherException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public IQueueMessage QueueMessage { get; private set; }
    }
}
