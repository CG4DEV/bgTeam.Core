namespace bgTeam.ElasticSearch
{
    using System;

    /// <summary>
    /// Throws elasticsearch exceptions
    /// </summary>
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
    }
}
