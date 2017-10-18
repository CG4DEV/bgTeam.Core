namespace Trcont.Ris.Story.Contracts
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;

    public class GetSaldoByClientGuidStory : IStory<GetSaldoByClientGuidStoryContext, ClientSaldoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetSaldoByClientGuidStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public ClientSaldoDto Execute(GetSaldoByClientGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<ClientSaldoDto> ExecuteAsync(GetSaldoByClientGuidStoryContext context)
        {
            if (context == null || context.ClientGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(context.ClientGuid));
            }

            var sql =
                @"SELECT
                  cl.ClientGUID,
                  SUM(ss.Saldo) AS Saldo
                FROM vClients cl
                  INNER JOIN Contract c ON c.ClientId = cl.Id
                  INNER JOIN SaldoServices ss ON c.IrsGuid = ss.ContractGuid
                WHERE cl.ClientGUID = @ClientGuid
                GROUP BY cl.ClientGUID";

            var res = await _repository.GetAsync<ClientSaldoDto>(sql, new { ClientGuid = context.ClientGuid });
            return res;
        }
    }
}
