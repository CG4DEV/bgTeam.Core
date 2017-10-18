namespace Trcont.Cud.Story.Common
{
    using System;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Cud.Domain.Dto;

    public class GetContractInfoByGuidStory : IStory<GetContractInfoByGuidStoryContext, ContractInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;

        public GetContractInfoByGuidStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
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

            var sql =
                @"SELECT
                  c1.ContractGUID,
                  c1.ContractNumber,             -- Номер договора
                  c1.ContractTitle,
                  c1.Status,
                  c1.CreateDate AS ContractDate, -- Дата договора
                  c1.BeginDate as ContractBeginDate,
                  c1.EndDate as ContractEndDate,
                  c1.AccType,
                  c1.BIK,
                  c1.BankTitle,
                  c1.BAddress AS BankAddress,
                  c.ClientGUID,
                  CASE
                     WHEN c.ClientType = 0 THEN ISNULL(c.FSecondName + ' ', '') + ISNULL(c.FSecondName + ' ', '') + ISNULL(c.FMiddleName + ' ', '')
                     ELSE c.JCompanyName
                  END AS ClientName,
                  c.ClientType,                  -- Форма собственности
                  c.JIndex,                      -- Юр индекс
                  c.JCity,                       -- Юр город
                  c.JAddress,                    -- Юр улица, дом, квартира / офис
                  c.FIndex,                      -- Почтовый индекс
                  c.FCity,                       -- Почтовый город
                  c.FAddress,                    -- Почтовый улица, дом, квартира / офис
                  c.Inn,                         -- ИНН
                  c.Kpp
                FROM Clients c
                INNER JOIN Contracts c1
                  ON c.ClientGUID = c1.ClientGUID
                WHERE c1.ContractGUID = @ContractGuid";

            var res = await _repository.GetAsync<ContractInfoDto>(sql, new { ContractGuid = context.ContractGuid });
            return res;
        }
    }
}
