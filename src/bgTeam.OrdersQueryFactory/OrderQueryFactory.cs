namespace bgTeam.OrdersQueryFactory
{
    using bgTeam.ProcessMessages;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using bgTeam.OrdersQueryFactory.Queries;
    using bgTeam.ProduceMessages;
    using System.Collections.Generic;
    using Dapper;

    public class OrderQueryFactory : IQueryFactory
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly IEntityMapService _entityMapService;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        public OrderQueryFactory(
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

        public string ForMessageType => "Orders";

        public IQuery CreateQuery(IQueueMessage msg)
        {
            return CreateQueryAsync(msg).Result;
        }

        public async Task<IQuery> CreateQueryAsync(IQueueMessage msg)
        {
            var map = _entityMapService.CreateEntityMap(msg);

            if (Convert.ToBoolean(map.Properties["IsTeo"]))
            {
                var ordersServiceDemand = GetDemand(map, "TeoService", "TeoId");
                // TODO : раскомментировать, когда решится проблема с процедурой
                // var ordersFactDemand = GetDemand(map, "OrdersFact", "TeoId"); 
                var osp = GetDemand(map, "TeoServiceParams", "TeoId");
                SendDemandQueuesAsync(ordersServiceDemand, /*ordersFactDemand,*/ osp);

                if (await GetIsOrderExistAsync(map))
                {
                    return new UpdateOrderQuery(_factory, map);
                }
                else
                {
                    var kp = GetDemand(map, "KP", "Id");
                    SendDemandQueuesAsync(kp);
                    return new InsertOrderQuery(_factory, map);
                }
            }
            else
            {
                var ordersServiceDemand = GetDemand(map, "KpService", "Id");
                var kpsp = GetDemand(map, "KPServiceParams", "Id");
                SendDemandQueuesAsync(ordersServiceDemand, kpsp);

                if (await GetIsOrderExistAsync(map))
                {
                    if (!(await GetIsTeoExistAsync(map)))
                    {
                        return new UpdateOrderQuery(_factory, map);
                    }
                    else if (map.Properties.ContainsKey("TeoGuid") && !string.IsNullOrWhiteSpace(map.Properties["TeoGuid"].ToString()))
                    {
                        var kp = GetDemand(map, "KP", "Id");
                        SendDemandQueuesAsync(kp);
                        return new UpdateTeoByOrder(_factory, map);
                    }
                }
                else
                {
                    return new InsertOrderQuery(_factory, map);
                }
            }

            return new EmptyQuery();
        }

        private async Task<bool> GetIsTeoExistAsync(EntityMap map)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN TeoGuid IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM {map.TypeName}
                         WHERE {map.KeyName} = @Id";

            return await _repository.GetAsync<bool>(sql, new { Id = map.KeyValue });
        }

        private async Task<bool> GetIsOrderExistAsync(EntityMap map)
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

        private async void SendDemandQueuesAsync(params Dictionary<string, object>[] entitys)
        {
            foreach (var e in entitys)
            {
                await Task.Run(() => _sender.Send(e, e["ConfigName"].ToString()));
            }
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
