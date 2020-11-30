namespace bgTeam.Queues.Exceptions
{
    using System;

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
    }
}
