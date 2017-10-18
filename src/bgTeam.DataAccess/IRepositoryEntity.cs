namespace bgTeam.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRepositoryEntity
    {
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class;

        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class;

        //Task InsertAcync<T>(T entity)
        //    where T : class;

        Task InsertAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        Task<bool> UpdateAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        Task<bool> DeleteAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;
    }
}
