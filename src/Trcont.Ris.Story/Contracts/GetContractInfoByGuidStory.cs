namespace Trcont.Ris.Story.Common
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Threading.Tasks;
    using Trcont.Ris.DataAccess.Contracts;
    using Trcont.Ris.Domain.Dto;

    public class GetContractInfoByGuidStory : IStory<GetContractInfoByGuidStoryContext, ContractInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetContractInfoByGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public ContractInfoDto Execute(GetContractInfoByGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<ContractInfoDto> ExecuteAsync(GetContractInfoByGuidStoryContext context)
        {
            if (!context.ContractGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.ContractGuid));
            }

            var sql = @"
            SELECT
              c1.Id,
              c1.IrsGuid AS ContractGUID,
              c1.RegNumber AS ContractNumber,
              c1.Name AS ContractTitle,
              c1.CudStatus AS Status,
              c1.ContractDate,
              c1.BeginDate as ContractBeginDate,
              c1.EndDate as ContractEndDate,
              c1.AccType,
              c1.BIK,
              c1.BankTitle,
              c1.BankAddress,
              c.ClientGUID,
              c.ClientName,
              c.ClientType,
              c.JIndex,
              c.JCity,
              c.JAddress,
              c.FIndex,
              c.FCity,
              c.FAddress,
              c.Inn,
              c.Kpp,
              ISNULL((SELECT SUM(Saldo) FROM SaldoServices ss WHERE ss.ContractGuid = c1.IrsGuid), 0) AS Summ
            FROM vClients c
            INNER JOIN Contract c1
              ON c.Id = c1.ClientId
            WHERE c1.IrsGuid = @ContractGuid";

            var contract = await _repository.GetAsync<ContractInfoDto>(sql, new { ContractGuid = context.ContractGuid });
            if (contract != null)
            {
                var cmd = new GetContractParamsCmd(_repository);
                contract.Params = await cmd.ExecuteAsync(new GetContractParamsCmdContext() { ContractId = contract.Id });
            }

            return contract;
        }
    }
}
