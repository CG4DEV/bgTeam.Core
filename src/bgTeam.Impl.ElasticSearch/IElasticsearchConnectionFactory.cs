namespace bgTeam.Impl.ElasticSearch
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
        ElasticClient CreateClient();

        /// <summary>
        /// Create new client instance
        /// </summary>
        Task<ElasticClient> CreateClientAsync();
    }
}
