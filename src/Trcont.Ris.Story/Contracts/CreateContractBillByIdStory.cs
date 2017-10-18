namespace Trcont.Ris.Story.Contracts
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.App.Service;
    using Trcont.Domain;
    using Trcont.Ris.DataAccess.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class CreateContractBillByIdStory : IStory<CreateContractBillByIdStoryContext, Documents>
    {
        private const string GET_REFERENCE_BY_GUID = "Info/GetReferenceByIRSGuid";

        private readonly IAppLogger _logger;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IRepository _repository;
        private readonly IAppServiceClient _appService;
        private readonly IWebClient _webClient;
        private readonly IMapperBase _mapper;

        public CreateContractBillByIdStory(
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

        public Documents Execute(CreateContractBillByIdStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<Documents> ExecuteAsync(CreateContractBillByIdStoryContext context)
        {
            if (!context.ContractId.HasValue)
            {
                throw new ArgumentNullException(nameof(context.ContractId));
            }

            if (!context.UserGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.UserGuid));
            }

            if (context.AccountBillItems == null)
            {
                throw new ArgumentNullException(nameof(context.AccountBillItems));
            }

            var contract = await _repositoryEn.GetAsync<Contract>(x => x.Id == context.ContractId);
            if (contract == null)
            {
                throw new ApplicationException($"Not find contract id - {context.ContractId}");
            }

            int[] indexes = context.AccountBillItems.OrderBy(x => x.Index).Select(x => x.Index).ToArray();
            decimal[] billSumms = context.AccountBillItems.OrderBy(x => x.Index).Select(x => x.Summ).ToArray();

            // Создаём счет
            var res = await _appService.CreateContractAccountBillAsync(contract.IrsGuid, indexes, billSumms, context.UserGuid.Value, context.IsClient);

            // Получаем из ирсы данные по созданному счёту
            var billIrs = await _webClient.PostAsync<IEnumerable<Reference>>(GET_REFERENCE_BY_GUID, new { ReferenceGuids = new[] { res } });
            var doc = _mapper.Map(billIrs.First(), new Documents());
            doc.TimeStamp = DateTime.Now;

            // Сохраняем документ и его связь с Договором
            var cmd = new SaveDocumentByEntityIdCmd(_repositoryEn, _repository);
            await cmd.ExecuteAsync(new SaveDocumentByEntityIdCmdContext()
            {
                EntityId = contract.Id.Value,
                EntityType = 2,
                Document = doc
            });

            return doc;
        }
    }
}
