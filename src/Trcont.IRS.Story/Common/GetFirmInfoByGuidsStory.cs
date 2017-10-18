namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.IRS.Domain.Dto;

    public class GetFirmInfoByGuidsStory : IStory<GetFirmInfoByGuidsStoryContext, IEnumerable<FirmInfoDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetFirmInfoByGuidsStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<FirmInfoDto> Execute(GetFirmInfoByGuidsStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<FirmInfoDto>> ExecuteAsync(GetFirmInfoByGuidsStoryContext context)
        {
            if (context.ClientGuids == null || !context.ClientGuids.Any())
            {
                return Enumerable.Empty<FirmInfoDto>();
            }

            var sql =
               @"  SELECT
                     r.ReferenceGUID,
                     r.ReferenceTitle AS Name,
                     rf.FullName,
                     rf.RegisterType,
                     rf.CodeINN AS 'INN',
                     rf.CodeOKPO AS 'OKPO',
                     rf.CodeKPP AS 'KPP',
                     rf.CodeOGRN AS 'OGRN',
                     rf.ChiefFirm AS 'Director'
                  FROM RefFirms rf
                  INNER JOIN Reference r ON rf.ReferenceId = r.ReferenceId
                  WHERE r.ReferenceGUID IN (SELECT Id FROM @ClientGuids)
            ";

            return await _repository.GetAllAsync<FirmInfoDto>(sql, new { ClientGuids = new GuidDbType(context.ClientGuids) });
        }
    }
}
