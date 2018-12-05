namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Elasticsearch.Net;
    using Nest;

    public class ElasticsearchConnectionFactory : IElasticsearchConnectionFactory
    {
        private readonly ConnectionSettings _connectionSettings;

        public ElasticsearchConnectionFactory(IElasticsearchConnectionSettings setting)
        {
            var staticConnectionPool = new StaticConnectionPool(setting.Nodes.Select(x => new Uri(x)));
            _connectionSettings = new ConnectionSettings(staticConnectionPool);
        }

        public ElasticClient CreateClient()
        {
            return CreateClientAsync().Result;
        }

        public async Task<ElasticClient> CreateClientAsync()
        {
            return new ElasticClient(_connectionSettings);
        }
    }
}
