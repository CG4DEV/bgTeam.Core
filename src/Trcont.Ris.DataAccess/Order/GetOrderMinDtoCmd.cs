namespace Trcont.Ris.DataAccess.Order
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using Trcont.Ris.Domain.Dto;

    public class GetOrderMinDtoCmd : ICommand<GetOrderMinDtoCmdContext, OrderMinDto>
    {
        private readonly IRepository _repository;

        public GetOrderMinDtoCmd(IRepository repository)
        {
            _repository = repository;
        }

        public OrderMinDto Execute(GetOrderMinDtoCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrderMinDto> ExecuteAsync(GetOrderMinDtoCmdContext context)
        {
            return await LoadOrderAsync(context.OrderId);
        }

        private async Task<OrderMinDto> LoadOrderAsync(int id)
        {
            var sql = @"
        SELECT 
             o.Id,
             o.TeoId,
             o.Number,                        -- Номер
             ct.IrsGuid AS TrainTypeGuid,
             o.StatusId,                      -- Статус
             o.PlaceFromId,
             placeFrom.IrsGuid AS PlaceFromGuid,
             placeFrom.PointType AS PlaceFromPointType,
             o.PlaceToId,
             placeTo.IrsGuid AS PlaceToGuid,
             placeTo.PointType AS PlaceToPointType,
             o.ContainerQuantity
        FROM Orders o WITH (NOLOCK)
          LEFT JOIN vPoints placeFrom WITH (NOLOCK) ON o.PlaceFromId = placeFrom.Id
          LEFT JOIN vPoints placeTo WITH (NOLOCK) ON o.PlaceToId = placeTo.Id
          LEFT JOIN ContainerType ct WITH (NOLOCK) ON o.TrainTypeId = ct.Id
        WHERE o.Id = @OrderId";

            return await _repository.GetAsync<OrderMinDto>(sql, new { OrderId = id });
        }
    }
}
