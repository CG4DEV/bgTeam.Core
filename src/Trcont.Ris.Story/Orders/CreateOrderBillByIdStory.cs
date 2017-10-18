namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Web;
    using Trcont.App.Service;
    using Trcont.Domain;
    using Trcont.Ris.Domain.Entity;
    using System.Collections.Generic;
    using System.Linq;
    using Trcont.Ris.DataAccess.Common;

    public class CreateOrderBillByIdStory : IStory<CreateOrderBillByIdStoryContext, Documents>
    {
        private readonly IAppLogger _logger;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IRepository _repository;
        private readonly IAppServiceClient _appService;
        private readonly IWebClient _webClient;
        private readonly IMapperBase _mapper;

        private const string GET_REFERENCE_BY_GUID = "Info/GetReferenceByIRSGuid";

        public CreateOrderBillByIdStory(
            IAppLogger logger,
            IRepositoryEntity repositoryEn,
            IRepository repository,
            IAppServiceClient appService,
            IWebClient webClient,
            IMapperBase mapper)
        {
            _logger = logger;
            _repositoryEn = repositoryEn;
            _appService = appService;
            _webClient = webClient;
            _mapper = mapper;
            _repository = repository;
        }

        public Documents Execute(CreateOrderBillByIdStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<Documents> ExecuteAsync(CreateOrderBillByIdStoryContext context)
        {
            if (!context.OrderId.HasValue)
            {
                throw new ArgumentNullException(nameof(context.OrderId));
            }

            if (!context.UserGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.UserGuid));
            }

            var order = await _repositoryEn.GetAsync<Order>(x => x.Id == context.OrderId);
            if (order == null)
            {
                throw new ApplicationException($"Not find order id - {context.OrderId}");
            }

            if (!order.TeoGuid.HasValue)
            {
                throw new ApplicationException($"Not find order id - {context.OrderId}, teo is null");
            }

            // Создаём счет
            var res = await _appService.CreateOrderBillAsync(order.TeoGuid.Value, new Guid("0a35f2ac-bc9e-4062-91f3-35d566c9533e"), context.UserGuid.Value);

            // Получаем из ирсы данные по созданному счёту
            var billIrs = await _webClient.PostAsync<IEnumerable<Reference>>(GET_REFERENCE_BY_GUID, new { ReferenceGuids = new[] { res } });
            var doc = _mapper.Map(billIrs.First(), new Documents());
            doc.TimeStamp = DateTime.Now;

            // Сохраняем документ и его связь с КП
            var cmd = new SaveDocumentByEntityIdCmd(_repositoryEn, _repository);
            await cmd.ExecuteAsync(new SaveDocumentByEntityIdCmdContext()
            {
                EntityId = order.Id.Value,
                EntityType = 1,
                Document = doc
            });

            return doc;
        }
    }
}
