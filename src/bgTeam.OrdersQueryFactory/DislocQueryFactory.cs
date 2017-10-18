namespace bgTeam.OrdersQueryFactory
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.ProcessMessages;
    using bgTeam.Queues;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.OrdersQueryFactory.Queries;

    public class DislocQueryFactory : IQueryFactory
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly IEntityMapService _entityMapService;

        public string ForMessageType => "RailStation";

        public DislocQueryFactory(IRepository repository, IConnectionFactory factory, IEntityMapService entityMapService)
        {
            _repository = repository.CheckNull(nameof(repository));
            _factory = factory.CheckNull(nameof(factory));
            _entityMapService = entityMapService.CheckNull(nameof(entityMapService));
        }

        public IQuery CreateQuery(IQueueMessage msg)
        {
            return CreateQueryAsync(msg).Result;
        }

        public async Task<IQuery> CreateQueryAsync(IQueueMessage msg)
        {
            var entityMap = _entityMapService.CreateEntityMap(msg);

            if (await GetIsExistAsync(entityMap))
            {
                return new UpdateQuery(_factory, entityMap);
            }

            return new InsertQuery(_factory, entityMap);
        }

        private async Task<bool> GetIsExistAsync(EntityMap map)
        {
            var sql = $@"SELECT {(map.KeyName.Equals("IrsGuid")?"TOP 1":"")}
                            CASE
                              WHEN {map.KeyName} IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM {map.TypeName}
                         WHERE {map.KeyName} = @Id";

            return await _repository.GetAsync<bool>(sql, new { Id = map.KeyValue });
        }
    }
}
