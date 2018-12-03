namespace bgTeam
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class QueueWarningException : Exception
    {
        public QueueWarningException()
        {

        }

        public QueueWarningException(string message)
            : base(message)
        {

        }

        public QueueWarningException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected QueueWarningException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
