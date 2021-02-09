namespace bgTeam.ElasticSearch
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Elasticsearch client
    /// </summary>
    public interface IElasticsearchClient
    {
        /// <summary>
        /// Index document in elasticsearch
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="document">Document insert or update</param>
        /// <param name="indexName">Elasticsearch index</param>
        void Index<T>(T document, string indexName)
            where T : class;

        /// <summary>
        /// Index document in elasticsearch
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="document">Document insert or update</param>
        /// <param name="indexName">Elasticsearch index</param>
        Task IndexAsync<T>(T document, string indexName)
            where T : class;

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="id">Document id</param>
        /// <param name="indexName">Elasticsearch index</param>
        T Get<T>(string id, string indexName)
            where T : class;

        /// <summary>
        /// Get document by id
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="id">Document id</param>
        /// <param name="indexName">Elasticsearch index</param>
        Task<T> GetAsync<T>(string id, string indexName)
            where T : class;

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
        IEnumerable<T> Search<T>(string field, string value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

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
        Task<IEnumerable<T>> SearchAsync<T>(string field, string value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

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
        IEnumerable<T> Search<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

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
        Task<IEnumerable<T>> SearchAsync<T>(string field, DateTime? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

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
        IEnumerable<T> Search<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

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
        Task<IEnumerable<T>> SearchAsync<T>(string field, double? value, string indexName, string sortField = "Id", bool ascSort = false, int? size = null)
            where T : class;

        /// <summary>
        /// Full text search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="searchString">String with search request</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="size">Count to return</param>
        IEnumerable<T> FullTextSearch<T>(string searchString, string indexName, int? size = null)
            where T : class;

        /// <summary>
        /// Full text search documents
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="searchString">String with search request</param>
        /// <param name="indexName">Elasticsearch index</param>
        /// <param name="size">Count to return</param>
        Task<IEnumerable<T>> FullTextSearchAsync<T>(string searchString, string indexName, int? size = null)
            where T : class;
    }
}
