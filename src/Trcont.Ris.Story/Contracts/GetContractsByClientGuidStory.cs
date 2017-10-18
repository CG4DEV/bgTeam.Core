namespace Trcont.Ris.Story.Info
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using Trcont.Ris.Domain.Dto;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;

    public class GetContractsByClientGuidStory : IStory<GetContractsByClientGuidStoryContext, IEnumerable<ContractDto>>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetContractsByClientGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IEnumerable<ContractDto> Execute(GetContractsByClientGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<ContractDto>> ExecuteAsync(GetContractsByClientGuidStoryContext context)
        {
            if (context == null || context.ClientGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(GetContractsByClientGuidStoryContext.ClientGuid));
            }

            var sql =
                @"SELECT 
                  ct.Id,
                  ct.RegNumber,
                  ct.Name,
                  ct.IrsGuid,
                  ct.ContractDate,
                  ct.BeginDate,
                  ct.EndDate,
                  cs.Id AS StateId,
                  cs.Title AS StateName,
                  cl.Id AS ClientId,
                  cl.ClientName,
                  ISNULL((SELECT SUM(Saldo) FROM SaldoServices ss WHERE ss.ContractGuid = ct.IrsGuid), 0) AS Summ
                FROM vClients cl WITH(NOLOCK)
                  INNER JOIN Contract ct WITH(NOLOCK) ON cl.Id = ct.ClientId
                  LEFT JOIN ContractState cs WITH(NOLOCK) ON cs.Id = ct.ContractStateId
                WHERE cl.ClientGuid = @ClientGuid
                  ORDER BY ct.ContractDate";

            var res = await _repository.GetAllAsync<ContractDto>(sql, new { ClientGuid = context.ClientGuid });

            // TODO : Убрать после передачи фронт части
            res.DoForEach(x => x.IsValid = x.StateId == 207374);

            return res;
        }
    }
}
