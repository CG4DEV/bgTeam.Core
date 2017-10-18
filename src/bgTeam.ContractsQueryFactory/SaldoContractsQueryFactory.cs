namespace bgTeam.ContractsQueryFactory
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using System.Threading.Tasks;
    using bgTeam.Extensions;
    using Newtonsoft.Json;
    using bgTeam.ContractsQueryFactory.Dto;
    using bgTeam.ContractsQueryFactory.Queries;
    using System;
    using System.Collections.Generic;
    using DapperExtensions.Mapper;
    using System.Linq;

    public class SaldoContractsQueryFactory : IQueryFactory
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly IEntityMapService _entityMapService;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;
        private readonly IAppLogger _logger;

        public SaldoContractsQueryFactory(
            IAppLogger logger,
            IRepository repository,
            IConnectionFactory factory,
            IEntityMapService entityMapService,
            ISenderEntity sender,
            ISenderEntity notificationSender)
        {
            _logger = logger.CheckNull(nameof(logger));
            _repository = repository.CheckNull(nameof(repository));
            _factory = factory.CheckNull(nameof(factory));
            _entityMapService = entityMapService.CheckNull(nameof(entityMapService));
            _sender = sender.CheckNull(nameof(sender));
            _notificationSender = notificationSender.CheckNull(nameof(notificationSender));
        }

        public string ForMessageType => "SaldoContracts";

        public IQuery CreateQuery(IQueueMessage msg)
        {
            return CreateQueryAsync(msg).Result;
        }

        public async Task<IQuery> CreateQueryAsync(IQueueMessage msg)
        {
            var settings = new JsonSerializerSettings()
            {
                DateParseHandling = DateParseHandling.None,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            var obj = JsonConvert.DeserializeObject<SaldoInfoDto>(msg.Body, settings);

            if (!obj.Orders.NullOrEmpty())
            {
                await SetOrderIdsAsync(obj.Orders);
            }

            return new ProcessSaldoQuery(obj, _logger, _factory);
        }

        private async Task SetOrderIdsAsync(IEnumerable<SaldoOrderInfoDto> orders)
        {
            string sql = $@"
                SELECT o.Id, o.TeoGuid
                FROM Orders o
                WHERE o.TeoGuid IN (SELECT Id FROM @TeoGuids)";

            var teoGuids = orders.Select(x => x.RequestGuid).ToArray();
            var result = await _repository.GetAllAsync<OrderProxy>(sql,
                new { TeoGuids = new GuidDbType(teoGuids) });
            var dict = result.ToDictionary(x => x.TeoGuid);

            foreach (var order in orders)
            {
                if (dict.ContainsKey(order.RequestGuid))
                {
                    order.Id = dict[order.RequestGuid].Id;
                }
                else
                {
                    var demand = GetDemand(order, "Orders", "RequestGuid");
                    SendDemandQueuesAsync(demand);
                }
            }
        }

        private Dictionary<string, object> GetDemand(SaldoOrderInfoDto order, string configName, string columnName)
        {
            var confArgs = new Dictionary<string, object>();
            confArgs.Add("ConfigName", configName);
            confArgs.Add(columnName, order.RequestGuid);
            return confArgs;
        }

        private async void SendDemandQueuesAsync(params Dictionary<string, object>[] entitys)
        {
            foreach (var e in entitys)
            {
                await Task.Run(() => _sender.Send(e, e["ConfigName"].ToString()));
            }
        }
    }
}
