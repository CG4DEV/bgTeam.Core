namespace bgTeam.DataAccess.Impl.Dapper
{
    using System.Data;
    using System.Threading.Tasks;
    using DapperExtensions;
    using global::Dapper;

    public class CrudServiceDapper : ICrudService
    {
        private readonly IConnectionFactory _factory;

        public CrudServiceDapper(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public bool Insert<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    connection.Insert(entity, transaction: transaction);
                }
            }
            else
            {
                connection.Insert(entity, transaction: transaction);
            }

            return true;
        }

        public async Task<bool> InsertAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    await connection.InsertAcync(entity, transaction: transaction);
                }
            }
            else
            {
                await connection.InsertAcync(entity, transaction: transaction);
            }

            return true;
        }

        public bool Update<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Update(entity, transaction: transaction);
                }
            }
            else
            {
                return connection.Update(entity, transaction: transaction);
            }
        }

        public async Task<bool> UpdateAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.UpdateAcync(entity, transaction: transaction);
                }
            }
            else
            {
                return await connection.UpdateAcync(entity, transaction: transaction);
            }
        }

        public bool Delete<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Delete(entity, transaction: transaction);
                }
            }
            else
            {
                return connection.Delete(entity, transaction: transaction);
            }
        }

        // TODO : необходимо реализовать async для connection.Delete
        public async Task<bool> DeleteAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return connection.Delete(entity, transaction: transaction);
                }
            }
            else
            {
                return connection.Delete(entity, transaction: transaction);
            }
        }

        public int Execute(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Execute(obj.Sql, obj.QueryParams, transaction: transaction);
                }
            }
            else
            {
                return connection.Execute(obj.Sql, obj.QueryParams, transaction: transaction);
            }
        }

        public async Task<int> ExecuteAsync(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.ExecuteAsync(obj.Sql, obj.QueryParams, transaction: transaction);
                }
            }
            else
            {
                return await connection.ExecuteAsync(obj.Sql, obj.QueryParams, transaction: transaction);
            }
        }

        public int Execute(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Execute(sql, param, transaction: transaction);
                }
            }
            else
            {
                return connection.Execute(sql, param, transaction: transaction);
            }
        }

        public async Task<int> ExecuteAsync(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.ExecuteAsync(sql, param, transaction: transaction);
                }
            }
            else
            {
                return await connection.ExecuteAsync(sql, param, transaction: transaction);
            }
        }
    }
}
