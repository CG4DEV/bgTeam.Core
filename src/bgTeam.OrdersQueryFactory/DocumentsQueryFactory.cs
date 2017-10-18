namespace bgTeam.OrdersQueryFactory
{
    using bgTeam.DataAccess;
    using bgTeam.ProcessMessages;
    using bgTeam.ProduceMessages;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using bgTeam.Extensions;
    using bgTeam.OrdersQueryFactory.Queries;

    public class DocumentsQueryFactory : IQueryFactory
    {
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly IEntityMapService _entityMapService;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        public DocumentsQueryFactory(
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

        public string ForMessageType => "Documents";

        public IQuery CreateQuery(IQueueMessage msg)
        {
            return CreateQueryAsync(msg).Result;
        }

        public async Task<IQuery> CreateQueryAsync(IQueueMessage msg)
        {
            var map = _entityMapService.CreateEntityMap(msg);

            var isDocumentExists = await GetIsDocumentExistAsync(map);
            if (isDocumentExists.HasValue && isDocumentExists.Value)
            {
                return new UpdateDocumentsQuery(_repository, _factory, map);
            }

            return new InsertDocumentsQuery(_repository, _factory, map);
        }

        private async Task<bool?> GetIsDocumentExistAsync(EntityMap map)
        {
            var sql = $@"SELECT 
                            CASE
                              WHEN Id IS NOT NULL THEN 1
                              ELSE 0
                            END
                         FROM {map.TypeName}
                         WHERE {map.KeyName} = @Id";

            return await _repository.GetAsync<bool?>(sql, new { Id = map.KeyValue });
        }
    }
}
