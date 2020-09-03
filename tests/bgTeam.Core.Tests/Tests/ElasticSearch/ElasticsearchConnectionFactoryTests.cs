using bgTeam.ElasticSearch;
using bgTeam.Impl.ElasticSearch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.ElasticSearch
{
    public class ElasticsearchConnectionFactoryTests
    {
        [Fact]
        public void DependencyElasticsearchConnectionSettings()
        {
            Assert.Throws<ArgumentNullException>("setting", () => new ElasticsearchConnectionFactory(null));
        }

        [Fact]
        public void DependencyElasticsearchConnectionSettingNodes()
        {
            Assert.Throws<ArgumentOutOfRangeException>("setting", () => new ElasticsearchConnectionFactory(new Settings()));
            Assert.Throws<ArgumentOutOfRangeException>("setting", () => new ElasticsearchConnectionFactory(new Settings { Nodes = new string[0] }));
        }

        [Fact]
		public void CreateClient()
		{
			var elasticsearchConnectionFactory = new ElasticsearchConnectionFactory(new Settings { Nodes = new[] { "http://test.com" } });
            var client = elasticsearchConnectionFactory.CreateClient();
            Assert.NotNull(client);
        }

        [Fact]
        public async Task CreateClientAsync()
        {
            var elasticsearchConnectionFactory = new ElasticsearchConnectionFactory(new Settings { Nodes = new[] { "http://test.com" } });
            var client = await elasticsearchConnectionFactory.CreateClientAsync();
            Assert.NotNull(client);
        }
    }

    class Settings : IElasticsearchConnectionSettings
    {
        public IEnumerable<string> Nodes { get; set; }
    }
}
