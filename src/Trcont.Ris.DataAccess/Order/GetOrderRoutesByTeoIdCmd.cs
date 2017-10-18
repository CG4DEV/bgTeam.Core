namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetOrderRoutesByTeoIdCmd : ICommand<GetOrderRoutesByTeoIdCmdContext, IEnumerable<OrderRouteDto>>
    {
        private readonly IRepository _repository;

        public GetOrderRoutesByTeoIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<OrderRouteDto> Execute(GetOrderRoutesByTeoIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<OrderRouteDto>> ExecuteAsync(GetOrderRoutesByTeoIdCmdContext context)
        {
            var sql = @"
            SELECT 
              orts.Id,
              orts.TeoId,
              orts.OrderId,
              orts.FromPointId,
              fPoint.IrsGuid AS FromPointGUID,
              ISNULL(fPoint.CnsiCode,orts.FromPointCode) AS FromPointCode,
              ISNULL(fPoint.Title,orts.FromPointTitle) AS FromPointTitle,
              fCountry.Id AS FromCountryId,
              fCountry.IrsGuid AS FromCountryGuid,
              fCountry.Title AS FromCountryTitle,
              orts.ToPointId,
              tPoint.IrsGuid AS ToPointGUID, 
              ISNULL(tPoint.CnsiCode,orts.ToPointCode) AS ToPointCode,
              ISNULL(tPoint.Title,orts.ToPointTitle) AS ToPointTitle,
              tCountry.Id AS ToCountryId,
              tCountry.IrsGuid AS ToCountryGuid,
              tCountry.Title AS ToCountryTitle,
              orts.RouteType,
              orts.ArmIndex,
              orts.CreateDate,
              orts.TimeStamp
            FROM OrdersRoutes orts WITH(NOLOCK)
              LEFT JOIN vPoints fPoint WITH(NOLOCK) ON orts.FromPointId = fPoint.Id
              LEFT JOIN Country fCountry WITH(NOLOCK) ON fPoint.CountryId = fCountry.Id
              LEFT JOIN vPoints tPoint WITH(NOLOCK) ON orts.ToPointId = tPoint.Id
              LEFT JOIN Country tCountry WITH(NOLOCK) ON tPoint.CountryId = tCountry.Id
            WHERE orts.TeoId = @TeoId
            ORDER BY orts.ArmIndex ASC
            ";

            return await _repository.GetAllAsync<OrderRouteDto>(sql, context);
        }
    }
}
