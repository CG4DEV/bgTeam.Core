namespace bgTeam.DataAccess
{
    using System.Data;
    using System.Threading.Tasks;

    public interface ICrudService
    {
        bool Insert<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        Task<bool> InsertAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        bool Update<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        Task<bool> UpdateAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        bool Delete<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        Task<bool> DeleteAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        int Execute(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        Task<int> ExecuteAsync(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        int Execute(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        Task<int> ExecuteAsync(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);
    }
}
