namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ElasticsearchException : Exception
    {
        public ElasticsearchException()
            : base()
        {
        }

        public ElasticsearchException(string message)
            : base(message)
        {
        }

        public ElasticsearchException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
