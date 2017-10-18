namespace Trcont.Cud.DataAccess.Stations
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Common;
    using Trcont.Cud.Domain.Dto;

    public class GetStationByZoneStrCmd : ICommand<GetStationByZoneStrCmdContext, IEnumerable<StationInfoDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IAddressISales _addressService;

        public GetStationByZoneStrCmd(
            IAppLogger logger,
            IRepository repository,
            IAddressISales addressService)
        {
            _logger = logger;
            _repository = repository;
            _addressService = addressService;
        }

        public IEnumerable<StationInfoDto> Execute(GetStationByZoneStrCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<StationInfoDto>> ExecuteAsync(GetStationByZoneStrCmdContext context)
        {
            if (string.IsNullOrEmpty(context.ZoneStr))
            {
                throw new ArgumentNullException(nameof(context.ZoneStr));
            }

            var info = _addressService.GetStructure(context.ZoneStr);

            var str = new StringBuilder();

            if (info.CityType != null)
            {
                str.Append("AND CityType = @cityType ");
            }

            if (info.CityTitle != null)
            {
                str.Append("AND CityTitle = @cityTitle ");
            }

            if (info.TownType != null)
            {
                str.Append("AND TownType = @townType ");
            }

            if (info.TownTitle != null)
            {
                str.Append("AND TownTitle = @townTitle ");
            }

            if (info.StreetType != null)
            {
                str.Append("AND StreetType = @streetType ");
            }

            if (info.StreetTitle != null)
            {
                str.Append("AND StreetTitle = @streetTitle ");
            }

            var sql = str.ToString();
            if (string.IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(context.ZoneStr));
            }

            var res = await _repository.GetAllAsync<StationInfoDto>(
                $@"WITH zones AS
                      (SELECT DISTINCT TOP 1 ZoneId, a.ZoneName
                       FROM dbo.Address a
                       INNER JOIN Region r ON a.RegionId = r.RegionId
                       WHERE r.ExternalCode = @regionGuid {sql}
                       ORDER BY ZoneId DESC )
                    SELECT DISTINCT p.PointId,
                                    p.Title,
                                    p.ExternalCode,
                                    p.PointGuid AS StationGuid,
                                    p.CountryGuid,
                                    z.ZoneName as ZoneGuid
                    FROM zoneTopoint zp
                    INNER JOIN Point p ON zp.PointId = p.PointId
                    INNER JOIN zones z ON zp.ZoneId = z.ZoneId",
                new
                {
                    info.RegionGuid,
                    info.CityType,
                    info.CityTitle,
                    info.TownType,
                    info.TownTitle,
                    info.StreetType,
                    info.StreetTitle,
                });

            foreach (var item in res)
            {
                item.IsAutoDelivery = true;
                item.ZoneStr = context.ZoneStr;
                item.ZoneTitle = _addressService.GetAddress(info);
            }

            return res;
        }
    }
}
