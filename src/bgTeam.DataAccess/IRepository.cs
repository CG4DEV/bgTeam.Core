namespace bgTeam.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;

    public interface IRepository
    {
        T Get<T>(ISqlObject obj);

        Task<T> GetAsync<T>(ISqlObject obj);

        T Get<T>(string sql, object param = null);

        Task<T> GetAsync<T>(string sql, object param = null);

        ////Task<T> GetEntityAsync<T>(Expression<Func<T, bool>> predicate)
        ////    where T : class;

        IEnumerable<T> GetAll<T>(ISqlObject obj);

        Task<IEnumerable<T>> GetAllAsync<T>(ISqlObject obj);

        IEnumerable<T> GetAll<T>(string sql, object param = null);

        Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param = null);

        ////Task<IEnumerable<T>> GetAllEntityAsync<T>(Expression<Func<T, bool>> predicate = null)
        ////    where T : class;

        ////System.Data.DataTable SelectDataTable(string sql);

        Task<int> ExecuteAsync(string sql, object param = null);
    }
}
