namespace bgTeam
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class BgTeamException : Exception
    {
        public BgTeamException()
        {

        }

        public BgTeamException(string message)
            : base(message)
        {

        }

        public BgTeamException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        protected BgTeamException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
