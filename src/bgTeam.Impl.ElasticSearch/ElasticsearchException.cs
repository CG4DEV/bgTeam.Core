namespace bgTeam.ElasticSearch
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Throws elasticsearch exceptions
    /// </summary>
    [Serializable]
    public class ElasticsearchException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchException"/> class.
        /// </summary>
        public ElasticsearchException()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchException"/> class.
        /// </summary>
        /// <param name="message"></param>
        public ElasticsearchException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchException"/> class.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ElasticsearchException(string message, Exception ex)
            : base(message, ex)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchException"/> class.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ElasticsearchException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
