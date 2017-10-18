namespace Trcont.Ris.Story.Contracts
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Web;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.App.Service;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetContractServceTypesStory : IStory<GetContractServceTypesStoryContext, IEnumerable<AccountBillItemDto>>
    {
        private const string GET_REFERENCE_BY_GUID = "Info/GetReferenceByIRSGuid";

        private readonly IAppLogger _logger;
        private readonly IRepositoryEntity _repository;
        private readonly IAppServiceClient _appService;
        private readonly IWebClient _webClient;
        private readonly IMapperBase _mapper;

        public GetContractServceTypesStory(
            IAppLogger logger,
            IRepositoryEntity repository,
            IAppServiceClient appService,
            IWebClient webClient,
            IMapperBase mapper)
        {
            _logger = logger;
            _repository = repository;
            _appService = appService;
            _webClient = webClient;
            _mapper = mapper;
        }

        public IEnumerable<AccountBillItemDto> Execute(GetContractServceTypesStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<AccountBillItemDto>> ExecuteAsync(GetContractServceTypesStoryContext context)
        {
            var vatTypes = (await _repository.GetAllAsync<VatType>()).OrderBy(x => x.Title);
            var contractServiceType = (await _repository.GetAllAsync<ContractServiceType>()).OrderBy(x => x.Title);
            var accBillItems = new List<AccountBillItemDto>();
            var index = 0;

            foreach (var servType in contractServiceType)
            {
                foreach (var vatType in vatTypes)
                {
                    accBillItems.Add(new AccountBillItemDto()
                    {
                        Summ = 0M,
                        Title = $"{servType.Title}({vatType.Title})",
                        Index = ++index
                    });
                }
            }

            return accBillItems;
        }
    }
}
