namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Elasticsearch.Net;
    using Nest;

    public class ConnectionFactoryEs : IConnectionFactoryEs
    {
        private readonly IConnectionSettingsEs _setting;
        private readonly StaticConnectionPool _staticConnectionPool;
        private readonly ConnectionSettings _connectionSettings;

        public ConnectionFactoryEs(IConnectionSettingsEs setting)
        {
            _setting = setting;
            _staticConnectionPool = new StaticConnectionPool(setting.Nodes.Select(x => new Uri(x)));
            _connectionSettings = new ConnectionSettings(_staticConnectionPool);
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
