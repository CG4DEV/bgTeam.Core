namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Ris.Common;
    using Trcont.Common.Utils;
    using Trcont.Ris.DataAccess.Order;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;
    using bgTeam.Extensions;
    using Trcont.Ris.DataAccess.Common;

    public class GetOrderTransPicStory : IStory<GetOrderTransPicStoryContext, TransPicDto>
    {
        private const int _SERVICE21012 = 1533173; // берётся из TK_Options
        private const int _SERVICE21015 = 1533180; // берётся из TK_Options
        private const int _SERVICECOMPLEX10203 = 58290099; // берётся из TK_ServiceOptions

        private static readonly Dictionary<string, Guid> _placeParamGuids = new Dictionary<string, Guid>()
        {
            { "4e", new Guid("0000004e-0000-0000-0000-000000000000") },
            { "5b", new Guid("0000005b-0000-0000-0000-000000000000") },
            { "4f", new Guid("0000004f-0000-0000-0000-000000000000") },
            { "5c", new Guid("0000005c-0000-0000-0000-000000000000") }
        };

        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;
        private readonly ITransPicService _transPicService;

        public GetOrderTransPicStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper,
            ITransPicService transPicService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _transPicService = transPicService;
        }

        public TransPicDto Execute(GetOrderTransPicStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<TransPicDto> ExecuteAsync(GetOrderTransPicStoryContext context)
        {
            var orderMinCmd = new GetOrderMinDtoCmd(_repository);
            OrderMinDto order = await orderMinCmd.ExecuteAsync(new GetOrderMinDtoCmdContext() { OrderId = context.OrderId });
            IEnumerable<OrderRouteDto> routes;

            if (order == null)
            {
                return null;
            }

            if (order.TeoId.HasValue)
            {
                var routesCmd = new GetOrderRoutesByTeoIdCmd(_repository);
                routes = await routesCmd.ExecuteAsync(new GetOrderRoutesByTeoIdCmdContext()
                {
                    TeoId = order.TeoId.Value
                });
            }
            else
            {
                throw new Exception($"У КП с идентификатором {order.Id} не имеет заказа");
            }

            IEnumerable<FactByRouteDto> facts = null;
            if (order.TeoId.HasValue)
            {
                var factCmd = new GetFactsByTeoIdCmd(_repository);
                facts = await factCmd.ExecuteAsync(new GetFactsByTeoIdCmdContext() { OrderId = order.TeoId.Value });
            }

            order.Routes = routes;
            order.TransPic = _transPicService.GetTransPicString(order, facts);

            return _mapper.Map(order, new TransPicDto());
        }
    }
}
