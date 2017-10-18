namespace bgTeam.Infrastructure.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using DapperExtensions;

    public class RepositoryEntityDapper : IRepositoryEntity
    {
        private readonly IConnectionFactory _factory;

        public RepositoryEntityDapper(IConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate)
            where T : class
        {
            var list = await GetAllAsync(predicate);

            return list.SingleOrDefault();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null)
            where T : class
        {
            using (var connection = await _factory.CreateAsync())
            {
                return await connection.GetAllAsync(predicate);
            }
        }

        public async Task InsertAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
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

        // TODO : необходимо реализовать async
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
    }
}
