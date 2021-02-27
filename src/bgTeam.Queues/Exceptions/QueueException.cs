namespace bgTeam.Queues.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class QueueException : Exception
    {
        public QueueException()
        {
        }

        public QueueException(string message)
            : base(message)
        {
        }

        public QueueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected QueueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
