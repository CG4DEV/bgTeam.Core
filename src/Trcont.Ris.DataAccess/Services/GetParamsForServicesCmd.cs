namespace Trcont.Ris.DataAccess.Common
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.DataAccess.Helpers;

    public class GetParamsForServicesCmd : ICommand<GetParamsForServicesCmdContext, IEnumerable<ServiceParamDto>>
    {
        private readonly IRepository _repository;

        public GetParamsForServicesCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ServiceParamDto> Execute(GetParamsForServicesCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ServiceParamDto>> ExecuteAsync(GetParamsForServicesCmdContext context)
        {
            var sql = @"SELECT 
                          sp.*,
                          s.Id AS ServiceId,
                          s.IrsGuid AS ServiceGuid,
                          s.Title AS ServiceName
                        FROM ServiceParams sp
                        INNER JOIN ServiceToParams stp ON sp.ParamGuid = stp.ServiceParamsId
                        INNER JOIN Service s ON stp.ServiceId = s.Id
                        WHERE s.Id in (SELECT id FROM @SrvIds)
                        ORDER BY sp.Code";
            var result = await _repository.GetAllAsync<ServiceParamDto>(sql, new { SrvIds = new IntDbType(context.ServiceIds) });

            return result.TrimNameRus();
        }
    }
}
