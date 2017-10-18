namespace Trcont.IRS.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.IRS.Domain.Dto;
    using Trcont.IRS.Domain.Entity;

    public class GetFactTransportInfoByTeoGuidStory : IStory<GetFactTransportInfoByTeoGuidStoryContext, IEnumerable<FactTransportByTeoGuidDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;

        public GetFactTransportInfoByTeoGuidStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<FactTransportByTeoGuidDto> Execute(GetFactTransportInfoByTeoGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<FactTransportByTeoGuidDto>> ExecuteAsync(GetFactTransportInfoByTeoGuidStoryContext context)
        {
            var factInfoCollection = await GetFactTransportInfoAsync(context.ReferenceGuids);

            var result = new List<FactTransportByTeoGuidDto>();
            foreach (var guid in context.ReferenceGuids)
            {
                var info = new FactTransportByTeoGuidDto()
                {
                    ReferenceGuid = guid
                };

                info.FactTransportCollection = factInfoCollection
                    .Where(x => x.ReferenceGuid == guid)
                    .Select(x => 
                    {
                        var fact = new FactTransport();
                        return _mapper.Map(x, fact);
                    })
                    .ToArray();

                result.Add(info);
            }

            return result;
        }

        private async Task<IEnumerable<FactInfoDto>> GetFactTransportInfoAsync(IEnumerable<Guid> referenceGuids)
        {
            var sql = @"
            select DISTINCT
                r.ReferenceGuid,
                ft.FactTransId,
                ft.ReportDate,
                ft.KontNumber,
                ft.WagonNumber,
                ft.OutDate,
                ft.ComDate,
                ft.FactSource as FactSourceId,
                ft.NaklNumber
            from detailtransport df
                inner join reference r on df.referenceid = r.referenceid
                inner join exectransport ef on ef.detailtransid = df.detailtransid and ef.isprofit = 1
                inner join exectrans2facttrans e2f on e2f.exectransid = ef.exectransid
                inner join facttransport ft on ft.facttransid = e2f.facttransid
            where r.referenceguid IN (SELECT Id FROM @ReferenceGuids) and df.detailtype = 2
            ";

            return await _repository.GetAllAsync<FactInfoDto>(sql, new { ReferenceGuids = new GuidDbType(referenceGuids) });
        }
    }
}
