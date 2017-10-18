namespace bgTeam.OrdersQueryFactory.Queries
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Dapper;
    using System.Data;

    public abstract class AbstractDocumentsQuery : AbstractQuery, IQuery
    {
        private readonly IRepository _repository;

        protected IEnumerable<string> DocumentsColumns => OrderedColumns.Except(new[] { "OwnerTypeId", "OwnerId" });

        public AbstractDocumentsQuery(IRepository repository, IConnectionFactory factory, EntityMap map)
            : base (factory, map)
        {
            _repository = repository;
        }

        public async Task ProcessOwnerAsync(IDbConnection connection)
        {
            var isOwnerExists = await GetIsOwnerExistAsync(connection);
            if (isOwnerExists.HasValue && isOwnerExists.Value)
            {
                return;
            }

            await InsertOwnerAsync(connection);
        }

        public async Task ProcessConnectionAsync(IDbConnection connection)
        {
            var isConnectionExists = await GetIsConnectionExistAsync(connection);
            if (isConnectionExists.HasValue && isConnectionExists.Value)
            {
                return;
            }

            await InsertConnectionAsync(connection);
        }

        private async Task InsertConnectionAsync(IDbConnection connection)
        {
            var sql = $"INSERT INTO Documents2Entity (OwnerId, DocumentId) VALUES(@OwnerId, @DocumentId)";

            await connection.ExecuteAsync(sql, new
            {
                OwnerId = _map.Properties["OwnerId"],
                DocumentId = _map.Properties["Id"]
            }, commandTimeout: COMMAND_TIMEOUT);
        }

        private async Task<bool?> GetIsConnectionExistAsync(IDbConnection connection)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN Id IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM Documents2Entity
                         WHERE OwnerId = @OwnerId AND DocumentId = @DocumentId";

            //return await _repository.GetAsync<bool?>();
            return await connection.ExecuteScalarAsync<bool?>(sql, new { OwnerId = _map.Properties["OwnerId"], DocumentId = _map.Properties["Id"] }, commandTimeout: COMMAND_TIMEOUT);
        }

        private async Task<bool?> GetIsOwnerExistAsync(IDbConnection connection)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN Id IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM Entities
                         WHERE Id = @Id";

            //return await _repository.GetAsync<bool?>();
            return await connection.ExecuteScalarAsync<bool?>(sql, new { Id = _map.Properties["OwnerId"] }, commandTimeout: COMMAND_TIMEOUT);
        }

        private async Task InsertOwnerAsync(IDbConnection connection)
        {
            var sql = $"INSERT INTO Entities (Id, OwnerTypeId) VALUES(@Id, @OwnerTypeId)";

            await connection.ExecuteAsync(sql, new
                {
                    Id = _map.Properties["OwnerId"],
                    OwnerTypeId = _map.Properties["OwnerTypeId"]
                }, 
                commandTimeout: COMMAND_TIMEOUT);
        }
    }
}
