namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public class GetOrdersFactsByTeoIdCmd : ICommand<GetOrdersFactsByTeoIdCmdContext, FactObjectDto>
    {
        private readonly IRepository _repository;

        public GetOrdersFactsByTeoIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public FactObjectDto Execute(GetOrdersFactsByTeoIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<FactObjectDto> ExecuteAsync(GetOrdersFactsByTeoIdCmdContext context)
        {
            if (!context.Order.ContainerQuantity.HasValue)
            {
                return null;
            }

            var facts = await GetFactsAsync(context);
            if (!facts.Any())
            {
                return null;
            }

            return GetFactObject(context.Order, facts);
        }

        public async Task<IEnumerable<OrderFact>> GetFactsAsync(GetOrdersFactsByTeoIdCmdContext context)
        {
            return await _repository.GetAllAsync<OrderFact>(
               @"SELECT
                    *
                FROM OrdersFact
                WHERE OrderId = @TeoId",
                new { TeoId = context.Order.TeoId });
        }

        private FactObjectDto GetFactObject(OrderMinDto order, IEnumerable<OrderFact> facts)
        {
            var mainStartedCount = facts.Count(x => x.TransDate.HasValue && x.FactSourceId == FactSourceEnum.SourceNaklId);
            var mainDoneCount = facts.Count(x => x.ArrivalDate.HasValue && x.FactSourceId == FactSourceEnum.SourceNaklId);

            FactObjectDto fact = new FactObjectDto()
            {
                AutoFromDone = facts.Count(x =>
                    !string.IsNullOrWhiteSpace(x.WagonNumber) &&
                    ((x.FactSourceId == FactSourceEnum.KEU16Id && order.PlaceFromId == x.TransStationFromId) ||
                     (x.FactSourceId == FactSourceEnum.KEO16OutSourceId) ||
                     (x.FactSourceId == FactSourceEnum.TTNId && order.PlaceFromId == x.TransStationFromId) ||
                     (x.FactSourceId == FactSourceEnum.TTNOutSourceId))
                     ) >= order.ContainerQuantity.Value,
                AutoToDone = facts.Count(x =>
                    !string.IsNullOrWhiteSpace(x.WagonNumber) &&
                    ((x.FactSourceId == FactSourceEnum.KEU16Id && order.PlaceToId == x.TransStationFromId) ||
                     (x.FactSourceId == FactSourceEnum.KEO16InSourceId) ||
                     (x.FactSourceId == FactSourceEnum.TTNId && order.PlaceToId == x.TransStationFromId) ||
                     (x.FactSourceId == FactSourceEnum.TTNInSourceId))
                     ) >= order.ContainerQuantity.Value,
                MainStarted = mainStartedCount == 0
                        ? MainState.NotStarted
                        : mainStartedCount < order.ContainerQuantity.Value
                            ? MainState.InTransit
                            : MainState.Arrive,
                MainDone = mainDoneCount == 0
                        ? MainState.NotStarted
                        : mainDoneCount < order.ContainerQuantity.Value
                            ? MainState.InTransit
                            : MainState.Arrive,
            };

            return fact;
        }
    }
}
