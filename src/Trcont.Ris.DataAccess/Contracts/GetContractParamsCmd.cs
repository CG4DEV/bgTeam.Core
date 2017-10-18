namespace Trcont.Ris.DataAccess.Contracts
{
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class GetContractParamsCmd : ICommand<GetContractParamsCmdContext, IEnumerable<ContractParam>>
    {
        private readonly IRepository _repository;

        public GetContractParamsCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<ContractParam> Execute(GetContractParamsCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ContractParam>> ExecuteAsync(GetContractParamsCmdContext context)
        {
            string sql = @"
            SELECT
              cp.Id,
              cp.ParamId,
              cp.ContractId,
              cpn.ParameterTitle AS ParamTitle,
              cp.ParamName,
              cp.DefValue1,
              cp.ParamInt1,
              cp.ParamStr1,
              cp.ParamDateTime1,
              cp.ParamFloat1,
              cp.DefValue2,
              cp.ParamInt2,
              cp.IsVisible,
              cp.CreateDate,
              cp.TimeStamp
            FROM ContractParams cp WITH(NOLOCK)
              INNER JOIN Contract c WITH(NOLOCK) ON c.Id = cp.ContractId
              LEFT JOIN ContractParamsName cpn WITH(NOLOCK) ON cp.ParamName = cpn.ParameterName AND cpn.TemplateId = c.TemplateId
            WHERE cp.ContractId = @ContractId    
            ";

            return await _repository.GetAllAsync<ContractParam>(sql, context);
        }
    }
}
