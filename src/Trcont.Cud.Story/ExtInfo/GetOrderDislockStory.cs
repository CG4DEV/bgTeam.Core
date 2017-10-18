namespace Trcont.Cud.Story.ExtInfo
{
    using bgTeam;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.CitTrans.Service;
    using Trcont.Cud.Domain.Entity.ExtInfo;

    public class GetOrderDislockStory : IStory<GetOrderDislockStoryContext, IEnumerable<DislocationInfo>>
    {
        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;
        private readonly ICitTransServiceClient _client;

        public GetOrderDislockStory(IAppLogger logger, IMapperBase mapper, ICitTransServiceClient client)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client;
        }

        public IEnumerable<DislocationInfo> Execute(GetOrderDislockStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<DislocationInfo>> ExecuteAsync(GetOrderDislockStoryContext context)
        {
            var dislocations = await _client.GetOrderDislokAsync(context.OrderNumber, context.ContainerNumber);
            if (dislocations == null)
            {
                return null;
            }

            return dislocations.Select(x => _mapper.Map(x, new DislocationInfo())).ToArray();
        }
    }
}
