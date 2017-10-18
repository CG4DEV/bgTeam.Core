namespace Trcont.Cud.Story.Orders
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Web.Dto;

    public class GetBillsInfoByOrderGuidStory : IStory<GetBillsInfoByOrderGuidStoryContext, IEnumerable<BillInfo>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;
        private readonly IWebClient _webClient;

        private const string GET_FACT_TRANSPORT_BYGUID = "api/Info/GetFactTransportInfoByTeoGuid";

        public GetBillsInfoByOrderGuidStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper,
            IWebClient webClient
            )
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _webClient = webClient;
        }

        public IEnumerable<BillInfo> Execute(GetBillsInfoByOrderGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<BillInfo>> ExecuteAsync(GetBillsInfoByOrderGuidStoryContext context)
        {
            var facts = await _webClient
                .PostAsync<IEnumerable<FactResponse>>(GET_FACT_TRANSPORT_BYGUID, new { ReferenceGuids = new[] { context.ReferenceGuid } }) ?? Enumerable.Empty<FactResponse>();

            var fact = facts.FirstOrDefault();
            if (fact == null)
            {
                return Enumerable.Empty<BillInfo>();
            }

            if (fact.FactTransportCollection == null || !fact.FactTransportCollection.Any())
            {
                return Enumerable.Empty<BillInfo>();
            }

            return fact.FactTransportCollection
                .Select(x => _mapper.Map(x, new BillInfo()))
                .ToArray();
        }
    }
}
