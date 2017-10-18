namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class GetOrderServicesByDocumentIdCmd : ICommand<GetOrderServicesByDocumentIdCmdContext, IEnumerable<OrderServicesDto>>
    {
        private readonly IRepository _repository;

        public GetOrderServicesByDocumentIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<OrderServicesDto> Execute(GetOrderServicesByDocumentIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<OrderServicesDto>> ExecuteAsync(GetOrderServicesByDocumentIdCmdContext context)
        {
            return await _repository.GetAllAsync<OrderServicesDto>(
               $@"SELECT
                    os.ServiceId,
                    os.OrderId,
                    os.ServiceTypeId,
                    s.Code AS ServiceCode,
                    s.Title AS ServiceTitle,
                    s.IrsGuid AS ServiceTypeGuid,
                    os.FromPointId,
                    placeFrom.IrsGuid AS FromPointGuid,
                    placeFrom.Title AS FromPointTitle,
                    placeFrom.CnsiCode AS FromPointCnsiCode,
                    placeFrom.CnsiGuid AS FromPointCnsiGuid,
                    placeFrom.CountryId AS FromCountryId,
                    countryFrom.IrsGuid AS FromCountryGuid,
                    countryFrom.Title AS FromCountryTitle, 
                    os.ToPointId,
                    placeTo.IrsGuid AS ToPointGuid,
                    placeTo.Title AS ToPointTitle,
                    placeTo.CnsiCode AS ToPointCnsiCode,
                    placeTo.CnsiGuid AS ToPointCnsiGuid,
                    placeTo.CountryId AS ToCountryId,
                    countryTo.Title AS ToCountryTitle, 
                    countryTo.IrsGuid AS ToCountryGuid, 
                    os.TerritoryId,
                    territory.Title AS TerritoryTitle,
                    territory.Account AS TerritoryCode,
                    os.CurrencyId,
                    cur.Account AS CurrencyCode,
                    cur.Title AS CurrencyTitle,
                    os.Tariff,
                    os.TariffVAT,
                    os.Summ,
                    os.SummVAT,
                    os.TariffType,
                    os.ContractId,
                    os.ArmIndex,
                    os.SrcVolume
                FROM OrdersService os
                  LEFT JOIN Service s WITH (NOLOCK) ON s.Id = os.ServiceTypeId
                  LEFT JOIN vPoints placeFrom WITH (NOLOCK) ON os.FromPointId = placeFrom.Id
                  LEFT JOIN vPoints placeTo WITH (NOLOCK) ON os.ToPointId = placeTo.Id
                  LEFT JOIN Country countryFrom WITH (NOLOCK) ON placeFrom.CountryId = countryFrom.Id
                  LEFT JOIN Country countryTo WITH (NOLOCK) ON placeTo.CountryId = countryTo.Id
                  LEFT JOIN Country territory WITH (NOLOCK) ON territory.Id = os.TerritoryId
                  LEFT JOIN Currency cur WITH (NOLOCK) ON cur.Id = os.CurrencyId
                WHERE os.OrderId = @OrderId {(context.SkipChildren ? "AND os.ParentTeoServiceId IS NULL" : string.Empty)}
                ORDER BY os.ArmIndex ASC",
                new { OrderId = context.OrderId });
        }
    }
}
