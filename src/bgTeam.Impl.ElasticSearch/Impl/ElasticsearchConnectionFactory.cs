namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Elasticsearch.Net;
    using Nest;

    /// <summary>
    /// Elasticsearch client factory
    /// </summary>
    public class ElasticsearchConnectionFactory : IElasticsearchConnectionFactory
    {
        private readonly ConnectionSettings _connectionSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchConnectionFactory"/> class.
        /// </summary>
        /// <param name="setting"></param>
        public ElasticsearchConnectionFactory(IElasticsearchConnectionSettings setting)
        {
            var staticConnectionPool = new StaticConnectionPool(setting.Nodes.Select(x => new Uri(x)));
            _connectionSettings = new ConnectionSettings(staticConnectionPool);
        }

        /// <summary>
        /// Create new client instance
        /// </summary>
        public ElasticClient CreateClient()
        {
            return CreateClientAsync().Result;
        }

        /// <summary>
        /// Create new client instance
        /// </summary>
        public async Task<ElasticClient> CreateClientAsync()
        {
            return new ElasticClient(_connectionSettings);
        }
    }
}
