namespace bgTeam.Impl.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.ElasticSearch;
    using Elasticsearch.Net;
    using Nest;

    /// <summary>
    /// Elasticsearch client
    /// </summary>
    public class ElasticsearchClient : IElasticsearchClient
    {
        private readonly IElasticsearchConnectionFactory _connectionFactoryEs;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticsearchClient"/> class.
        /// </summary>
        /// <param name="connectionFactoryEs">Client factory</param>
        public ElasticsearchClient(
            IElasticsearchConnectionFactory connectionFactoryEs)
        {
            _connectionFactoryEs = connectionFactoryEs ?? throw new ArgumentNullException(nameof(connectionFactoryEs));
        }

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="id">Document id</param>
        /// <param name="indexName">Elasticsearch index</param>
        public T Get<T>(string id, string indexName)
            where T : class
        {
            var client = _connectionFactoryEs.CreateClient();
            var path = new DocumentPath<T>(new Id(id))
                .Index(indexName);
            var result = client.Get<T>(path);

            ValidateGetResult(result);
            return result.Source;
        }

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="id">Document id</param>
        /// <param name="indexName">Elasticsearch index</param>
        public async Task<T> GetAsync<T>(string id, string indexName)
            where T : class
        {
            var client = await _connectionFactoryEs.CreateClientAsync();
            var path = new DocumentPath<T>(new Id(id))
                .Index(indexName);
            var result = await client.GetAsync<T>(path);

            ValidateGetResult(result);
            return result.Source;
        }

        /// <summary>
        /// Index document in elasticsearch
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="document">Document insert or update</param>
        /// <param name="indexName">Elasticsearch index</param>
        public void Index<T>(T document, string indexName)
            where T : class
        {
            var client = _connectionFactoryEs.CreateClient();
            var result = client.Index(document, x => x.Index(indexName));

            if (!result.IsValid)
            {
                throw new ElasticsearchException($"Ошибка добавления/обновления записи в индексе: {indexName}{Environment.NewLine}{result.DebugInformation}");
            }
        }

        /// <summary>
        /// Index document in elasticsearch
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="document">Document insert or update</param>
        /// <param name="indexName">Elasticsearch index</param>
        public async Task IndexAsync<T>(T document, string indexName)
            where T : class
        {
            var client = await _connectionFactoryEs.CreateClientAsync();
            var result = await client.IndexAsync(document, x => x.Index(indexName));

            if (!result.IsValid)
            {
                throw new ElasticsearchException($"Ошибка добавления/обновления записи в индексе: {indexName}{Environment.NewLine}{result.DebugInformation}");
            }
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public IEnumerable<T> Search<T>(string field, DateTime? value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            return Search<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public IEnumerable<T> Search<T>(string field, double? value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            return Search<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public IEnumerable<T> Search<T>(string field, string value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            var query = CreateSearchQuery<T>(field, value, indexName, sortField, ascSort, size);

            var result = SearchInternal(query);
            if (!result.IsValid)
            {
                throw new ElasticsearchException(result.DebugInformation);
            }

            return result.Documents.ToList();
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, DateTime? value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            return await SearchAsync<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, double? value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            return await SearchAsync<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        /// <summary>
        /// Search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="field">Document field name</param>
        /// <param name="value">Search value</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, string value, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            var query = CreateSearchQuery<T>(field, value, indexName, sortField, ascSort, size);

            var result = await SearchInternalAsync(query);
            if (!result.IsValid)
            {
                throw new ElasticsearchException(result.DebugInformation);
            }

            return result.Documents.ToList();
        }

        /// <summary>
        /// Full text search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="searchString">String with search request</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public IEnumerable<T> FullTextSearch<T>(string searchString, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            var query = CreateFullTextQuery<T>(searchString, indexName, sortField, ascSort, size);

            var result = SearchInternal(query);
            if (!result.IsValid)
            {
                throw new ElasticsearchException(result.DebugInformation);
            }

            return result.Documents.ToList();
        }

        /// <summary>
        /// Full text search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="searchString">String with search request</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="sortField">Document field name sorted by</param>
        /// <param name="ascSort">Order by asc</param>
        /// <param name="size">Count to return</param>
        public async Task<IEnumerable<T>> FullTextSearchAsync<T>(string searchString, string indexName, string sortField = null, bool ascSort = false, int? size = null)
            where T : class
        {
            var query = CreateFullTextQuery<T>(searchString, indexName, sortField, ascSort, size);

            var result = await SearchInternalAsync(query);
            if (!result.IsValid)
            {
                throw new ElasticsearchException(result.DebugInformation);
            }

            return result.Documents.ToList();
        }

        private static SearchDescriptor<T> CreateFullTextQuery<T>(string searchString, string indexName, string sortField, bool ascSort, int? size)
            where T : class
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> queryFunc = q =>
                q.QueryString(qs => qs.Query(searchString).AllowLeadingWildcard(true));

            return CreateQuery(indexName, sortField, ascSort, size, queryFunc);
        }

        private static SearchDescriptor<T> CreateSearchQuery<T>(string field, string value, string indexName, string sortField, bool ascSort, int? size)
            where T : class
        {
            Func<QueryContainerDescriptor<T>, QueryContainer> queryFunc = q =>
                q.Match(m => m.Field(field).Query(value));

            return CreateQuery(indexName, sortField, ascSort, size, queryFunc);
        }

        private static SearchDescriptor<T> CreateQuery<T>(string indexName, string sortField, bool ascSort, int? size, Func<QueryContainerDescriptor<T>, QueryContainer> queryFunc)
            where T : class
        {
            var query = new SearchDescriptor<T>(indexName)
                .Size(size)
                .Query(queryFunc);

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var asc = ascSort ? SortOrder.Ascending : SortOrder.Descending;
                query = query.Sort(s => s.Field(sortField, asc));
            }

            return query;
        }

        private static void ValidateGetResult<T>(GetResponse<T> result)
            where T : class
        {
            if (!result.IsValid)
            {
                var ex = (ElasticsearchClientException)result.OriginalException;

                if (ex.FailureReason.HasValue
                    && ex.FailureReason != PipelineFailure.BadResponse
                    && ex.Message.IndexOf("Status code 404") == -1)
                {
                    throw new ElasticsearchException($"Elasticsearch error.{Environment.NewLine}{result.DebugInformation}");
                }
            }
        }

        private async Task<ISearchResponse<T>> SearchInternalAsync<T>(SearchDescriptor<T> query)
            where T : class
        {
            var client = await _connectionFactoryEs.CreateClientAsync();
            return await client.SearchAsync<T>(query);
        }

        private ISearchResponse<T> SearchInternal<T>(SearchDescriptor<T> query)
            where T : class
        {
            var client = _connectionFactoryEs.CreateClient();
            return client.Search<T>(query);
        }
    }
}
