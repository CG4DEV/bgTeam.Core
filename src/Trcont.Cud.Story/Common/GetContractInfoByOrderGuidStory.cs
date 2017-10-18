namespace Trcont.Cud.Story.Common
{
    using System;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Cud.Domain.Dto;

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

            var sql =
                @"SELECT
                  c1.ContractGUID,
                  c1.ContractNumber,             -- Номер договора
                  c1.CreateDate AS ContractDate, -- Дата договора
                  c.ClientGUID,
                  c.ClientType,                  -- Форма собственности
                  c.JIndex,                      -- Юр индекс
                  c.JCity,                       -- Юр город
                  c.JAddress,                    -- Юр улица, дом, квартира / офис
                  c.FIndex,                      -- Почтовый индекс
                  c.FCity,                       -- Почтовый город
                  c.FAddress,                    -- Почтовый улица, дом, квартира / офис
                  c.Inn,                         -- ИНН
                  c.Kpp                          -- КПП
                FROM Clients c
                INNER JOIN Contracts c1
                  ON c.ClientGUID = c1.ClientGUID
                WHERE c1.ContractGUID IN (
                    SELECT d.ContractGUID FROM KPDocuments d WHERE d.ReferenceGUID = @OrderGuid
                        UNION
                      SELECT t.ContractGUID FROM TeoDocuments t WHERE t.ReferenceGUID = @OrderGuid)";

            return await _repository.GetAsync<ContractInfoDto>(sql, new { OrderGuid = context.OrderGuid });
        }
    }
}
