namespace bgTeam.ContractsQueryFactory
{
    using bgTeam.ProcessMessages;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using bgTeam.ContractsQueryFactory.Queries;
    using bgTeam.ProduceMessages;
    using System.Collections.Generic;
    using Dapper;

    public class ContractsQueryFactory : IQueryFactory
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly IEntityMapService _entityMapService;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        public ContractsQueryFactory(
            IRepository repository,
            IConnectionFactory factory,
            IEntityMapService entityMapService,
            ISenderEntity sender,
            ISenderEntity notificationSender)
        {
            _repository = repository.CheckNull(nameof(repository));
            _factory = factory.CheckNull(nameof(factory));
            _entityMapService = entityMapService.CheckNull(nameof(entityMapService));
            _sender = sender.CheckNull(nameof(sender));
            _notificationSender = notificationSender.CheckNull(nameof(notificationSender));
        }

        public string ForMessageType => "Contract";

        public IQuery CreateQuery(IQueueMessage msg)
        {
            return CreateQueryAsync(msg).Result;
        }

        public async Task<IQuery> CreateQueryAsync(IQueueMessage msg)
        {
            var entityMap = _entityMapService.CreateEntityMap(msg);
            var contractParams = GetDemand(entityMap, "ContractParams", "Id");

            var isExists = await GetIsExistAsync(entityMap);
            IQuery query = isExists
                ? (IQuery) new UpdateQuery(_factory, entityMap) 
                : (IQuery) new InsertQuery(_factory, entityMap);
            SendDemandQueuesAsync(contractParams);
            return query;
        }

        private async void SendDemandQueuesAsync(params Dictionary<string, object>[] entitys)
        {
            foreach (var e in entitys)
            {
                await Task.Run(() => _sender.Send(e, e["ConfigName"].ToString()));
            }
        }

        private async Task<bool> GetIsExistAsync(EntityMap map)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN {map.KeyName} IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM {map.TypeName}
                         WHERE {map.KeyName} = @Id";

            return await _repository.GetAsync<bool>(sql, new { Id = map.KeyValue });
        }

        private Dictionary<string, object> GetDemand(EntityMap map, string configName, params string[] columns)
        {
            var confArgs = new Dictionary<string, object>();
            confArgs.Add("ConfigName", configName);
            foreach (var column in columns)
            {
                confArgs.Add(column, map.Properties[column]);
            }

            return confArgs;
        }
    }
}
