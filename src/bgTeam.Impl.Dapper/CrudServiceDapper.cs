namespace bgTeam.DataAccess.Impl.Dapper
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using DapperExtensions;
    using global::Dapper;

    /// <inheritdoc/>
    public class CrudServiceDapper : RepositoryDapper, ICrudService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudServiceDapper"/> class.
        /// </summary>
        /// <param name="factory"></param>
        public CrudServiceDapper(IConnectionFactory factory)
            : base(factory)
        {
        }

        /// <inheritdoc/>
        public dynamic Insert<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Insert(entity, transaction: transaction);
                }
            }
            else
            {
                return connection.Insert(entity, transaction: transaction);
            }
        }

        /// <inheritdoc/>
        public async Task<dynamic> InsertAsync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.InsertAsync(entity, transaction: transaction);
                }
            }
            else
            {
                return await connection.InsertAsync(entity, transaction: transaction);
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<bool> UpdateAsync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.UpdateAsync(entity, transaction: transaction);
                }
            }
            else
            {
                return await connection.UpdateAsync(entity, transaction: transaction);
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.DeleteAsync(entity, transaction: transaction);
                }
            }
            else
            {
                return await connection.DeleteAsync(entity, transaction: transaction);
            }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public IEnumerable<dynamic> Query(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = _factory.Create())
                {
                    return connection.Query(sql, param, transaction: transaction);
                }
            }
            else
            {
                return connection.Query(sql, param, transaction: transaction);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<dynamic>> QueryAsync(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.QueryAsync(sql, param, transaction: transaction);
                }
            }
            else
            {
                return await connection.QueryAsync(sql, param, transaction: transaction);
            }
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null)
        {
            if (connection == null)
            {
                using (connection = await _factory.CreateAsync())
                {
                    return await connection.QueryAsync<T>(sql, param, transaction: transaction);
                }
            }
            else
            {
                return await connection.QueryAsync<T>(sql, param, transaction: transaction);
            }
        }
    }
}
