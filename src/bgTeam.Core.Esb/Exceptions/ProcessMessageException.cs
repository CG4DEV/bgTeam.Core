namespace bgTeam.Exceptions
{
    using bgTeam.Queues;
    using System;

    public class ProcessMessageException : Exception
    {
        public ProcessMessageException(IQueueMessage queueMessage, string message, Exception innerExeception)
            : base($"{message}\r\n{queueMessage.Body}\r\n", innerExeception)
        {
            QueueMessage = queueMessage;
        }

        public IQueueMessage QueueMessage { get; private set; }
    }
}
