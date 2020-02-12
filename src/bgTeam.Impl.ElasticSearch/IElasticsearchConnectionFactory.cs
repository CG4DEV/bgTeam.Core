namespace bgTeam.ElasticSearch
{
    using Nest;
    using System.Threading.Tasks;

    /// <summary>
    /// Elasticsearch client factory
    /// </summary>
    public interface IElasticsearchConnectionFactory
    {
        /// <summary>
        /// Create new client instance
        /// </summary>
        IElasticClient CreateClient();

        /// <summary>
        /// Create new client instance
        /// </summary>
        Task<IElasticClient> CreateClientAsync();
    }
}
