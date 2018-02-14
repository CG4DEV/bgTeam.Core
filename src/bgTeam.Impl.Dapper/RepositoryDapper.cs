namespace bgTeam.Infrastructure.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using Dapper;
    using DapperExtensions;

    public class RepositoryDapper : IRepository
    {
        private readonly int _commandTimeout = 300;
        private readonly IConnectionFactory _factory;

        public RepositoryDapper(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public T Get<T>(ISqlObject obj)
        {
            var list = GetAll<T>(obj);

            return list.SingleOrDefault();
        }

        public async Task<T> GetAsync<T>(ISqlObject obj)
        {
            var list = await GetAllAsync<T>(obj);

            return list.SingleOrDefault();
        }

        public T Get<T>(string sql, object param = null)
        {
            var list = GetAll<T>(sql, param);

            return list.SingleOrDefault();
        }

        public async Task<T> GetAsync<T>(string sql, object param = null)
        {
            var list = await GetAllAsync<T>(sql, param);

            return list.SingleOrDefault();
        }

        public IEnumerable<T> GetAll<T>(ISqlObject obj)
        {
            using (var connection = _factory.Create())
            {
                return connection.Query<T>(obj.Sql, obj.QueryParams, commandTimeout: _commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(ISqlObject obj)
        {
            using (var connection = await _factory.CreateAsync())
            {
                return await connection.QueryAsync<T>(obj.Sql, obj.QueryParams, commandTimeout: _commandTimeout);
            }
        }

        public IEnumerable<T> GetAll<T>(string sql, object param = null)
        {
            using (var connection = _factory.Create())
            {
                return connection.Query<T>(sql, param, commandTimeout: _commandTimeout);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param = null)
        {
            using (var connection = await _factory.CreateAsync())
            {
                return await connection.QueryAsync<T>(sql, param, commandTimeout: _commandTimeout);
            }
        }

        public T Get<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            using (var connection = _factory.Create())
            {
                return connection.Get<T>(predicate);
            }
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            using (var connection = await _factory.CreateAsync())
            {
                return await connection.GetAsync<T>(predicate);
            }
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            using (var connection = _factory.Create())
            {
                return connection.GetAll<T>(predicate);
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            using (var connection = await _factory.CreateAsync())
            {
                return await connection.GetAllAsync<T>(predicate);
            }
        }

        //public async Task<Dapper.SqlMapper.GridReader> QueryMultipleAsync(string sql, object param = null)
        //{
        //    using (var connection = await _factory.CreateAsync())
        //    {
        //        return await connection.QueryMultipleAsync(sql, param, commandTimeout: _commandTimeout);
        //    }
        //}

        //public async Task<int> ExecuteAsync(string sql, object param = null)
        //{
        //    using (var connection = await _factory.CreateAsync())
        //    {
        //        return await connection.ExecuteAsync(sql, param, commandTimeout: _commandTimeout);
        //    }
        //}
    }
}
