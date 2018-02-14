namespace bgTeam.Infrastructure.DataAccess
{
    using bgTeam.DataAccess;
    using Dapper;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class RepositoryDataTable : IRepositoryDataTable
    {
        private readonly int _commandTimeout = 900;
        private readonly IConnectionFactory _factory;

        public RepositoryDataTable(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public IEnumerable<dynamic> GetAll(string sql, object param = null)
        {
            IEnumerable<dynamic> result = null;
            using (var connection = _factory.Create())
            {
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        result = connection.Query<dynamic>(sql, param, transaction: transaction, commandTimeout: _commandTimeout);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }

        public async Task<IEnumerable<dynamic>> GetAllAsync(string sql, object param = null)
        {
            IEnumerable<dynamic> result = null;
            using (var connection = await _factory.CreateAsync())
            {
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    try
                    {
                        result = await connection.QueryAsync<dynamic>(sql, param, transaction: transaction, commandTimeout: _commandTimeout);
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

            return result;
        }
    }
}
