namespace Trcont.Ris.Story.Info
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.Web;
    using Trcont.Domain.Common;
    using Trcont.Ris.DataAccess.Contracts;

    public class GetSaldoByContractGuidStory : IStory<GetSaldoByContractGuidStoryContext, SaldoInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IWebClient _webClient;
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;

        private const string GET_SALDO_BY_CONTRACT_GUID = "Stp/SptGetCNTSummByContractGuid";

        public GetSaldoByContractGuidStory(
            IAppLogger logger,
            IWebClient webClient,
            IRepository repository,
            IConnectionFactory factory)
        {
            _logger = logger;
            _webClient = webClient;
            _repository = repository;
            _factory = factory;
        }

        public SaldoInfoDto Execute(GetSaldoByContractGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<SaldoInfoDto> ExecuteAsync(GetSaldoByContractGuidStoryContext context)
        {
            if (context.ContractGuid == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(context.ContractGuid));
            }

            var response = await LoadSaldoInfoAsync(context);

            if (response.Services.NullOrEmpty())
            {
                response = await _webClient.PostAsync<SaldoInfoDto>(GET_SALDO_BY_CONTRACT_GUID, context);
                response.ContractGuid = context.ContractGuid;

                await SetOrderIdsAsync(response);

                var cmdContext = new SaveSaldoInfoCmdContext()
                {
                    SaldoInfo = response
                };
                var cmd = new SaveSaldoInfoCmd(_factory);
                await cmd.ExecuteAsync(cmdContext);
            }

            return response;
        }

        private async Task<SaldoInfoDto> LoadSaldoInfoAsync(GetSaldoByContractGuidStoryContext context)
        {
            var result = new SaldoInfoDto()
            {
                ContractGuid = context.ContractGuid
            };

            result.Orders = await LoadSaldoOrdersAsync(context);
            result.Services = await LoadSaldoServicesAsync(context);

            return result;
        }

        private async Task<IEnumerable<SaldoServiceInfoDto>> LoadSaldoServicesAsync(GetSaldoByContractGuidStoryContext context)
        {
            var sql = @"
            SELECT
              ServiceId,
              ServiceTitle,
              PayAccountId,
              PayAccountTitle,
              CurrencyId,
              cur.Title AS CurrencyTitle,
              Forbidden,
              Saldo,
              Summa,
              [Index]
            FROM dbo.SaldoServices ss WITH(NOLOCK)
              LEFT JOIN dbo.Currency cur WITH(NOLOCK) ON cur.Id = ss.CurrencyId
            WHERE ss.ContractGuid = @ContractGuid
            ";

            return await _repository.GetAllAsync<SaldoServiceInfoDto>(sql, context);
        }

        private async Task<IEnumerable<SaldoOrderInfoDto>> LoadSaldoOrdersAsync(GetSaldoByContractGuidStoryContext context)
        {
            var sql = @"
                SELECT
                  so.OrderId AS Id,
                  so.ServiceId,
                  so.AccountId,
                  so.TeoGuid AS RequestGuid,
                  so.OrderTitle AS RequestTitle,
                  so.BlockSum,
                  so.MoveSum,
                  so.ResultBlockSum
                FROM dbo.SaldoOrders so WITH(NOLOCK)
                WHERE so.ContractGuid = @ContractGuid
            ";

            return await _repository.GetAllAsync<SaldoOrderInfoDto>(sql, context);
        }

        private async Task SetOrderIdsAsync(SaldoInfoDto saldo)
        {
            if (saldo.Orders.Any())
            {
                string sql = $@"SELECT o.Id, o.TeoGuid
                FROM Orders o
                WHERE o.TeoGuid IN ({string.Join(", ", saldo.Orders.Select(x => $"'{x.RequestGuid}'"))})";

                var result = await _repository.GetAllAsync<OrderProxy>(sql);
                var dict = result.ToDictionary(x => x.TeoGuid);

                foreach (var order in saldo.Orders)
                {
                    if (dict.ContainsKey(order.RequestGuid))
                    {
                        order.Id = dict[order.RequestGuid].Id;
                    }
                }
            }
        }

        private class OrderProxy
        {
            public int Id { get; set; }

            public Guid TeoGuid { get; set; }
        }
    }
}
