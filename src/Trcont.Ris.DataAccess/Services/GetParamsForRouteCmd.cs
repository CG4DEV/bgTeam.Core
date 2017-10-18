namespace Trcont.Ris.DataAccess.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.DataAccess.Helpers;

    public class GetParamsForRouteCmd : ICommand<GetParamsForRouteCmdContext, IEnumerable<MinServiceParamDto>>
    {
        private readonly IRepository _repository;

        public GetParamsForRouteCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<MinServiceParamDto> Execute(GetParamsForRouteCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<MinServiceParamDto>> ExecuteAsync(GetParamsForRouteCmdContext context)
        {
            if (!context.ServiceIds.Any())
            {
                throw new ArgumentNullException(nameof(context.ServiceIds));
            }

            var sql = @"SELECT DISTINCT 
                          sp.*  
                        FROM ServiceParams sp
                        INNER JOIN ServiceToParams stp ON sp.ParamGuid = stp.ServiceParamsId
                        INNER JOIN Service s ON stp.ServiceId = s.Id
                        WHERE s.Id in (SELECT id FROM @SrvIds)
                          AND sp.ParamGuid NOT IN (SELECT id FROM @IgnoreParams)
                          AND sp.IsEditUserValue = 1
                        ORDER BY sp.Code";

            var servicesParams = await _repository.GetAllAsync<MinServiceParamDto>(sql, new { SrvIds = new IntDbType(context.ServiceIds), IgnoreParams = new GuidDbType(context.IgnoreParams) });

            return servicesParams.TrimNameRus();
        }
    }
}
