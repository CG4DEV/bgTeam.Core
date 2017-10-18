namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Ris.DataAccess.Order;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Enums;

    public class GetFactsByOrderIdStory : IStory<GetFactsByOrderIdStoryContext, IEnumerable<OrderFactDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetFactsByOrderIdStory(IAppLogger logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<OrderFactDto> Execute(GetFactsByOrderIdStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<OrderFactDto>> ExecuteAsync(GetFactsByOrderIdStoryContext context)
        {
            if (!context.OrderId.HasValue)
            {
                throw new ArgumentNullException(nameof(context.OrderId));
            }

            var orderMinCmd = new GetOrderMinDtoCmd(_repository);
            var order = await orderMinCmd.ExecuteAsync(new GetOrderMinDtoCmdContext() { OrderId = context.OrderId.Value });
            var facts = await ExecuteQueryAsync(context.OrderId.Value);

            foreach (var fct in facts)
            {
                SetFactAutoFromTo(order, fct);
            }

            return facts;
        }

        private async Task<IEnumerable<OrderFactDto>> ExecuteQueryAsync(int orderId)
        {
            string sql = @" SELECT o.Id AS OrderId,
                                fct.OrderId AS TeoId,
                                o.Number AS OrderNumber,
                                fct.ContNumber,
                                fct.NaklNumber, 
                                fct.TransDate,
                                fct.ArrivalDate,
                                fct.WagonNumber,
                                fct.FactSourceId AS FactSource,
                                fct.TransStationFromId,
                                fromSt.Title AS StationFromName,
                                fct.TransStationToId,
                                toSt.Title AS StationToName
                            FROM Orders o WITH (NOLOCK)
                            INNER JOIN OrdersFact fct WITH (NOLOCK) ON fct.OrderId = o.TeoId
                            LEFT JOIN RailStation fromSt WITH (NOLOCK) ON fromSt.Id = fct.TransStationFromId
                            LEFT JOIN RailStation toSt WITH (NOLOCK) ON toSt.Id = fct.TransStationToId
                            WHERE o.Id = @OrderId AND REPLACE(fct.WagonNumber, '0', '') != ''
                            ORDER BY fct.TransDate, fct.FactAccessDate";
            return await _repository.GetAllAsync<OrderFactDto>(sql, new { OrderId = orderId });
        }

        private void SetFactAutoFromTo(OrderMinDto order, OrderFactDto fact)
        {
            if (!string.IsNullOrWhiteSpace(fact.WagonNumber))
            {
                if (fact.FactSource == FactSourceEnum.KEO16OutSourceId || fact.FactSource == FactSourceEnum.TTNOutSourceId ||
                    ((fact.FactSource == FactSourceEnum.KEU16Id || fact.FactSource == FactSourceEnum.TTNId)
                        && order.PlaceFromId == fact.TransStationFromId))
                {
                    fact.AutoOut = fact.WagonNumber ?? fact.AutoTo;
                    fact.WagonNumber = null;
                }
                else if (fact.FactSource == FactSourceEnum.KEO16InSourceId || fact.FactSource == FactSourceEnum.TTNInSourceId ||
                    ((fact.FactSource == FactSourceEnum.KEU16Id || fact.FactSource == FactSourceEnum.TTNId)
                        && order.PlaceToId == fact.TransStationFromId))
                {
                    fact.AutoTo = fact.WagonNumber ?? fact.AutoOut;
                    fact.WagonNumber = null;
                }
            }
        }
    }
}
