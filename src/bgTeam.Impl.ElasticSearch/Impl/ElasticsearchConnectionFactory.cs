namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.ElasticSearch;
    using bgTeam.Extensions;
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
            if (setting == null)
            {
                throw new ArgumentNullException(nameof(setting));
            }

            if (setting.Nodes.NullOrEmpty())
            {
                throw new ArgumentOutOfRangeException(nameof(setting), "Should be used one node at least");
            }

            var staticConnectionPool = new StaticConnectionPool(setting.Nodes.Select(x => new Uri(x)));
            _connectionSettings = new ConnectionSettings(staticConnectionPool);
        }

        /// <summary>
        /// Create new client instance
        /// </summary>
        public IElasticClient CreateClient()
        {
            return CreateClientAsync().Result;
        }

        /// <summary>
        /// Create new client instance
        /// </summary>
        public async Task<IElasticClient> CreateClientAsync()
        {
            return new ElasticClient(_connectionSettings);
        }
    }
}
