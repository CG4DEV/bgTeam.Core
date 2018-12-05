namespace bgTeam.Impl.ElasticSearch
{
    using Elasticsearch.Net;
    using Nest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ElasticsearchClient : IElasticsearchClient
    {
        private readonly IElasticsearchConnectionFactory _connectionFactoryEs;

        public ElasticsearchClient(
            IElasticsearchConnectionFactory connectionFactoryEs)
        {
            _connectionFactoryEs = connectionFactoryEs;
        }

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

        public IEnumerable<T> Search<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            return Search<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        public IEnumerable<T> Search<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            return Search<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

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

        public async Task<IEnumerable<T>> SearchAsync<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            return await SearchAsync<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class
        {
            return await SearchAsync<T>(field, value.ToString(), indexName, sortField, ascSort, size);
        }

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
