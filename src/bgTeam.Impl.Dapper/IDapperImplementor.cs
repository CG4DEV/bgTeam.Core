namespace DapperExtensions
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Mapper.Sql;

    public interface IDapperImplementor
    {
        ISqlGenerator SqlGenerator { get; }

        Task<T> GetAsync<T>(IDbConnection connection, object id, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        Task<IEnumerable<T>> GetAllAsync<T>(IDbConnection connection, object predicate, IList<ISort> sort, IDbTransaction transaction, int? commandTimeout, bool buffered)
            where T : class;

        IEnumerable<T> GetPage<T>(IDbConnection connection, object predicate, IList<ISort> sort, int page, int resultsPerPage, IDbTransaction transaction, int? commandTimeout, bool buffered)
            where T : class;

        IEnumerable<T> GetSet<T>(IDbConnection connection, object predicate, IList<ISort> sort, int firstResult, int maxResults, IDbTransaction transaction, int? commandTimeout, bool buffered)
            where T : class;

        void Insert<T>(IDbConnection connection, IEnumerable<T> entities, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        Task<dynamic> InsertAsync<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        Task<bool> UpdateAsync<T>(IDbConnection connection, T entity, IPredicate predicate, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        bool Delete<T>(IDbConnection connection, T entity, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        bool Delete<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        int Count<T>(IDbConnection connection, object predicate, IDbTransaction transaction, int? commandTimeout)
            where T : class;

        IMultipleResultReader GetMultiple(IDbConnection connection, GetMultiplePredicate predicate, IDbTransaction transaction, int? commandTimeout);
    }
}
