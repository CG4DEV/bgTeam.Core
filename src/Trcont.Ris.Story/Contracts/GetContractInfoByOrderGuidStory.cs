namespace Trcont.Ris.Story.Contracts
{
    using System;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.DataAccess.Contracts;

    public class GetContractInfoByOrderGuidStory : IStory<GetContractInfoByOrderGuidStoryContext, ContractInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;

        public GetContractInfoByOrderGuidStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public ContractInfoDto Execute(GetContractInfoByOrderGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<ContractInfoDto> ExecuteAsync(GetContractInfoByOrderGuidStoryContext context)
        {
            if (!context.OrderGuid.HasValue)
            {
                throw new ArgumentNullException(nameof(context.OrderGuid));
            }

            //var sql =
            //    @"SELECT
            //      c1.IrsGuid AS ContractGUID,
            //      c1.RegNumber AS ContractNumber,   -- Номер договора
            //      c1.ContractDate,                  -- Дата договора
            //      c.ClientGUID,
            //      c.ClientType,                     -- Форма собственности
            //      c.JIndex,                         -- Юр индекс
            //      c.JCity,                          -- Юр город
            //      c.JAddress,                       -- Юр улица, дом, квартира / офис
            //      c.FIndex,                         -- Почтовый индекс
            //      c.FCity,                          -- Почтовый город
            //      c.FAddress,                       -- Почтовый улица, дом, квартира / офис
            //      c.Inn,                            -- ИНН
            //      c.Kpp                             -- КПП
            //    FROM Clients c
            //    INNER JOIN Contracts c1
            //      ON c.ClientGUID = c1.ClientGUID
            //    WHERE c1.ContractGUID IN (
            //        SELECT d.ContractGUID FROM KPDocuments d WHERE d.ReferenceGUID = @OrderGuid
            //            UNION
            //          SELECT t.ContractGUID FROM TeoDocuments t WHERE t.ReferenceGUID = @OrderGuid
            //)";

            var sql = @"
                SELECT
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
                  c.Kpp
                FROM vClients c
                INNER JOIN Contract c1
                  ON c.Id = c1.ClientId
                WHERE c1.Id IN (SELECT d.ContractId FROM Orders d WHERE d.IrsGuid = @OrderGuid)
            ";

            var contract = await _repository.GetAsync<ContractInfoDto>(sql, new { OrderGuid = context.OrderGuid });
            if (contract != null)
            {
                var cmd = new GetContractParamsCmd(_repository);
                contract.Params = await cmd.ExecuteAsync(new GetContractParamsCmdContext() { ContractId = contract.Id });
            }

            return contract;
        }
    }
}
