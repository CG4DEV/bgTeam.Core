namespace bgTeam.Queues.Exceptions
{
    using System;

    [Serializable]
    public class QueueWatcherException : Exception
    {
        public IQueueMessage QueueMessage { get; private set; }

        public QueueWatcherException(string message, IQueueMessage queueMessage, Exception innerExeception)
            : base(message, innerExeception)
        {
            QueueMessage = queueMessage;

            this.Source = queueMessage.Body;
        }
    }
}
