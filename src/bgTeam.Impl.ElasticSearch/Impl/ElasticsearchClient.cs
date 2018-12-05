namespace bgTeam.Impl.ElasticSearch
{
    using Elasticsearch.Net;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
            _connectionFactoryEs = connectionFactoryEs;
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
        public IEnumerable<T> Search<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
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
        public IEnumerable<T> Search<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
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
        public IEnumerable<T> Search<T>(string field, string value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            var client = _connectionFactoryEs.CreateClient();
            var asc = ascSort ? Nest.SortOrder.Ascending : Nest.SortOrder.Descending;

            var result = client.Search<T>(x => x
                .Index(indexName)
                .Size(size)
                .Query(q => q.Match(m => m.Field(field).Query(value)))
                .Sort(s => s.Field(sortField, asc)));

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
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
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
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
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
        public async Task<IEnumerable<T>> SearchAsync<T>(string field, string value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            var client = await _connectionFactoryEs.CreateClientAsync();
            var asc = ascSort ? Nest.SortOrder.Ascending : Nest.SortOrder.Descending;

            var result = await client.SearchAsync<T>(x => x
                .Index(indexName)
                .Size(size)
                .Query(q => q.Match(m => m.Field(field).Query(value)))
                .Sort(s => s.Field(sortField, asc)));

            if (!result.IsValid)
            {
                throw new ElasticsearchException(result.DebugInformation);
            }

            return result.Documents.ToList();
        }
    }
}
