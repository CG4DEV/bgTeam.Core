namespace Trcont.Ris.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;

    public class GetAutoZoneByCnsiCodeStory : IStory<GetAutoZoneByCnsiCodeStoryContext, AutoZoneDto>
    {
        private readonly IRepository _repository;

        public GetAutoZoneByCnsiCodeStory(IRepository repository)
        {
            _repository = repository;
        }

        public AutoZoneDto Execute(GetAutoZoneByCnsiCodeStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<AutoZoneDto> ExecuteAsync(GetAutoZoneByCnsiCodeStoryContext context)
        {
            if (string.IsNullOrWhiteSpace(context.CnsiCode))
            {
                throw new ArgumentException("Парметр не может быть пустым", nameof(context.CnsiCode));
            }

            var sql = @"
                SELECT TOP 1
                  r.Title AS RegionName,
                  r.IrsGuid AS RegionCode,
                  za.CityType,
                  za.CityTitle,
                  za.TownType,
                  za.TownTitle,
                  za.StreetType,
                  za.StreetTitle,
                  z.CnsiCode
                FROM Zone z
                LEFT JOIN ZoneAddress za ON za.ZoneGuid = z.IrsGuid
                LEFT JOIN Region r ON r.IrsGuid = za.RegionGuid
                WHERE z.CnsiCode = @CnsiCode";

            var proxy = await _repository.GetAsync<Proxy>(sql, context);

            if (proxy == null)
            {
                return null;
            }

            var autoZone = new AutoZoneDto()
            {
                City = new AutoZoneDto.TypeValue()
                {
                    Title = proxy.CityTitle,
                    Type = proxy.CityType
                },
                Region = new AutoZoneDto.CodeValue()
                {
                    Code = proxy.RegionCode,
                    Name = proxy.RegionName
                },
                Street = new AutoZoneDto.TypeValue()
                {
                    Title = proxy.StreetTitle,
                    Type = proxy.StreetType
                },
                Town = new AutoZoneDto.TypeValue()
                {
                    Title = proxy.TownTitle,
                    Type = proxy.TownType
                },
                CnsiCode = proxy.CnsiCode
            };

            return autoZone;
        }

        private class Proxy
        {
            public string RegionName { get; set; }

            public Guid RegionCode { get; set; }

            public int? CityType { get; set; }

            public string CityTitle { get; set; }

            public int? TownType { get; set; }

            public string TownTitle { get; set; }

            public int? StreetType { get; set; }

            public string StreetTitle { get; set; }

            public string CnsiCode { get; set; }
        }
    }
}
