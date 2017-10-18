namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using Trcont.Cud.Domain.Entity;

    public class GetCNSIInfoByGuidsCmd : ICommand<GetCNSIInfoByGuidsCmdContext, IEnumerable<CNSIInfo>>
    {
        private readonly IRepository _repository;

        public GetCNSIInfoByGuidsCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<CNSIInfo> Execute(GetCNSIInfoByGuidsCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<CNSIInfo>> ExecuteAsync(GetCNSIInfoByGuidsCmdContext context)
        {
            return await _repository.GetAllAsync<CNSIInfo>(
                @"SELECT  
                    ReferenceGUID,
                    ReferenceAccount,
                    ExternalSourceId,
                    ExternalCode,
                    ReferenceTitle,
                    ReferenceId
                  FROM vMapCNSI WHERE ReferenceGUID IN (SELECT Id FROM @Guids)",
                new { @Guids = new GuidDbType(context.Guids.Where(x => x.HasValue).Select(x => x.Value).Distinct()) });
        }
    }
}
