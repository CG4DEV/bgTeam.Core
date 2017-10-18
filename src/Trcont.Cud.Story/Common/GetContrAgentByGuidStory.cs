namespace Trcont.Cud.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Dto;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Entity;
    using Trcont.Cud.Infrastructure;
    using DapperExtensions.Mapper;

    public class GetContrAgentByGuidStory : IStory<GetContrAgentByGuidStoryContext, ClientDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IStInfoRepository _stInfoRepository;

        public GetContrAgentByGuidStory(
            IAppLogger logger,
            IRepository repository,
            IStInfoRepository stInfoRepository)
        {
            _logger = logger;
            _repository = repository;
            _stInfoRepository = stInfoRepository;
        }

        public ClientDto Execute(GetContrAgentByGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<ClientDto> ExecuteAsync(GetContrAgentByGuidStoryContext context)
        {
            if (context.ClientGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(context.ClientGuid));
            }

            var sql = @"
                SELECT
                  c.ClientGUID,
                  c.ClientType,
                  c.JCompanyName,
                  c.Kpp,
                  c.JIndex,
                  c.JCountryGUID, -- наименование
                  c.JCity,
                  c.JRegion,
                  c.JAddress,
                  c.FPhone,
                  c.FFax,
                  c.FEmail,
                  c.Inn,
                  c.FSecondName,
                  c.FFirstName,
                  c.FMiddleName,
                  c.DocSeries,
                  c.DocNumber,
                  c.DocDataReceive,
                  c.DocIssuer,
                  c.FIndex,
                  c.FCountryGUID, -- наименование
                  c.FCity,
                  c.FRegion,
                  c.FAddress,
                  c.Okpo,
                  c.Okato,
                  c.Okopf,
                  c.Okfc,
                  c.JShortName
                FROM Clients c
                WHERE c.ClientGUID = @ClientGUID
            ";

            var client = await _repository.GetAsync<ClientDto>(sql, new { ClientGUID = context.ClientGuid });

            if (client == null)
            {
                return client;
            }

            await SetCountriesTitleAsync(client);
            return client;
        }

        private async Task SetCountriesTitleAsync(ClientDto client)
        {
            var countriesGuids = (new[] { client.JCountryGUID, client.FCountryGUID })
                .Where(x => x.HasValue && x.Value != Guid.Empty)
                .Select(x => x.Value)
                .Distinct()
                .ToArray();

            if (!countriesGuids.Any())
            {
                return;
            }

            var countries = await LoadCountriesAsync(countriesGuids);
            foreach (var c in countries)
            {
                if (client.JCountryGUID == c.CountryGuid)
                {
                    client.JCountryTitle = c.Name;
                }

                if (client.FCountryGUID == c.CountryGuid)
                {
                    client.FCountryTitle = c.Name;
                }
            }
        }

        private async Task<IEnumerable<Country>> LoadCountriesAsync(Guid[] countriesGuids)
        {
            var sql = $@"
                SELECT 
                    ReferenceGUID AS CountryGuid,
                    FullCountryTitle AS Name
                FROM RefCountry c
                WHERE c.ReferenceGUID IN ({string.Join(",", countriesGuids.Select(x => $"'{x}'"))})
            ";

            return await _stInfoRepository.GetAllAsync<Country>(sql);
        }
    }
}
