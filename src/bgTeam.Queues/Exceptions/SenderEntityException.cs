namespace bgTeam.Queues.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Exception for failed send
    /// </summary>
    [Serializable]
    public class SenderEntityException : Exception
    {
        public SenderEntityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SenderEntityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
