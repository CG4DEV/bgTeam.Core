namespace bgTeam.Impl.MongoDB
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using global::MongoDB.Driver;

    /// <summary>
    /// MongoDB repository for work with collection
    /// </summary>
    public interface IMongoDBRepository
    {
        /// <summary>
        /// Get first input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        T Get<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Get all input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate = null)
            where T : class;

        /// <summary>
        /// Get all input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class;

        /// <summary>
        /// Get page by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="skip">to offset pointer</param>
        /// <param name="limit">page count</param>
        /// <param name="predicate">filtering</param>
        Task<IEnumerable<T>> GetPageAsync<T>(int skip, int limit, Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Get page by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="skip">to offset pointer</param>
        /// <param name="limit">page count</param>
        /// <param name="sort">ordering by field name</param>
        /// <param name="predicate">array of filters</param>
        Task<IEnumerable<T>> GetPageAsync<T>(int skip, int limit, IList<ISort> sort, params Expression<Func<T, bool>>[] predicates)
            where T : class;

        /// <summary>
        /// Get first input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Insert one document. Desirable: class should has <see cref="MongoDB.Bson.Serialization.Attributes.BsonIdAttribute"/> field
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="document">Doc to insert.</param>
        void Insert<T>(T document)
            where T : class;

        /// <summary>
        /// Insert one document async. Desirable: class should has <see cref="MongoDB.Bson.Serialization.Attributes.BsonIdAttribute"/> field
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="document">Doc to insert.</param>
        Task InsertAsync<T>(T document)
            where T : class;

        /// <summary>
        /// Insert many document. Desirable: class should has <see cref="MongoDB.Bson.Serialization.Attributes.BsonIdAttribute"/> field
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="document">Doc to insert.</param>
        void InsertMany<T>(IEnumerable<T> documents)
            where T : class;

        /// <summary>
        /// Insert many document async. Desirable: class should has <see cref="MongoDB.Bson.Serialization.Attributes.BsonIdAttribute"/> field
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="document">Doc to insert.</param>
        Task InsertManyAsync<T>(IEnumerable<T> documents)
            where T : class;

        /// <summary>
        /// Get all input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <typeparam name="V">New type for return</typeparam>
        /// <param name="result">Func for converting T to V</param>
        /// <param name="predicate">filtering</param>
        Task<IEnumerable<V>> GetAllWithProjectionAsync<T, V>(Expression<Func<T, V>> result, Expression<Func<T, bool>> predicate = null)
            where T : class;

        /// <summary>
        /// Delete first input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        bool Delete<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Delete first input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        Task<bool> DeleteAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Delete all input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        bool DeleteMany<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Delete all input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        Task<bool> DeleteManyAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Update first input by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        /// <param name="entity">New document for replace</param>
        /// <param name="isUpsert">Insert document if not exist</param>
        bool Update<T>(Expression<Func<T, bool>> predicate, T entity, bool isUpsert = false)
            where T : class;

        /// <summary>
        /// Update first input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        /// <param name="entity">New document for replace</param>
        /// <param name="isUpsert">Insert document if not exist</param>
        Task<bool> UpdateAsync<T>(Expression<Func<T, bool>> predicate, T entity, bool isUpsert = false)
            where T : class;

        /// <summary>
        /// Update certain fileds on first input by filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="filter">Filter defenition</param>
        /// <param name="update">Update defenition</param>
        Task<bool> UpdateAsync<T>(FilterDefinition<T> filter, UpdateDefinition<T> update)
            where T : class;

        /// <summary>
        /// Count documents by filter
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">filtering</param>
        long Count<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        /// <summary>
        /// Count documents by array of filter async
        /// </summary>
        /// <typeparam name="T">Collection type</typeparam>
        /// <param name="predicate">array of filter</param>
        Task<long> CountAsync<T>(params Expression<Func<T, bool>>[] predicates)
            where T : class;
    }
}
