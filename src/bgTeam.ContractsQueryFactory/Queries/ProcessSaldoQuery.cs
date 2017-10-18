namespace bgTeam.ContractsQueryFactory.Queries
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.ProcessMessages;
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using bgTeam.ContractsQueryFactory.Dto;

    public class ProcessSaldoQuery : IQuery
    {
        private readonly SaldoInfoDto _info;
        private readonly IConnectionFactory _factory;
        private readonly IAppLogger _logger;

        protected const int COMMAND_TIMEOUT = 300;
        private const int STATES_COUNT = 8;

        private static readonly Dictionary<int, EmptySaldoServiceDto> _emptyRecDictionary = new Dictionary<int, EmptySaldoServiceDto>()
        {
            { 1, new EmptySaldoServiceDto() { Index = 1, PayAccountId = 67082, PayAccountTitle = "1. НДС 18%", ServiceId = 253154, ServiceTitle = "1. Услуги РЖД и других соисполнителей" } },
            { 2, new EmptySaldoServiceDto() { Index = 2, PayAccountId = 228854, PayAccountTitle = "2. НДС 0%", ServiceId = 253154, ServiceTitle = "1. Услуги РЖД и других соисполнителей" } },
            { 3, new EmptySaldoServiceDto() { Index = 3, PayAccountId = 95604, PayAccountTitle = "3. НДС не облагается", ServiceId = 253154, ServiceTitle = "1. Услуги РЖД и других соисполнителей" } },
            { 4, new EmptySaldoServiceDto() { Index = 4, PayAccountId = 67082, PayAccountTitle = "1. НДС 18%", ServiceId = 253155, ServiceTitle = "2. Транспортно-экспедиционное обслуживание - услуги ТрансКонтейнер" } },
            { 5, new EmptySaldoServiceDto() { Index = 5, PayAccountId = 228854, PayAccountTitle = "2. НДС 0%", ServiceId = 253155, ServiceTitle = "2. Транспортно-экспедиционное обслуживание - услуги ТрансКонтейнер" } },
            { 6, new EmptySaldoServiceDto() { Index = 6, PayAccountId = 95604, PayAccountTitle = "3. НДС не облагается", ServiceId = 253155, ServiceTitle = "2. Транспортно-экспедиционное обслуживание - услуги ТрансКонтейнер" } },
            { 9, new EmptySaldoServiceDto() { Index = 9, PayAccountId = 95604, PayAccountTitle = "3. НДС не облагается", ServiceId = 253156, ServiceTitle = "3. Штрафы, установленные ТрансКонтейнер" } },
            { 12, new EmptySaldoServiceDto() { Index = 12, PayAccountId = 95604, PayAccountTitle = "3. НДС не облагается", ServiceId = 718560, ServiceTitle = "4. Штрафы РЖД" } }
        };

        public ProcessSaldoQuery(SaldoInfoDto info, IAppLogger logger, IConnectionFactory factory)
        {
            _info = info;
            _factory = factory;
            _logger = logger;
        }

        public void Execute()
        {
            ExecuteAsync().Wait();
        }

        public async Task ExecuteAsync()
        {
            if (_info.ContractGuid == Guid.Empty)
            {
                throw new Exception("Не задан идентификатор договора");
            }

            if (_info.Services.NullOrEmpty())
            {
                _info.Services = GetEmptyServiceResult();
            }

            if (_info.Services.Count() < STATES_COUNT)
            {
                _info.Services = GetMissingServices();
            }

            if (_info.Services.Count() > STATES_COUNT)
            {
                var obj = JsonConvert.SerializeObject(_info);
                _logger.Debug($"Для договора с идентификатором {_info.ContractGuid} количество статей больше 8 ({_info.Services.Count()}), объект {obj}");
            }

            using (var connection = _factory.Create())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await RemoveOldSaldoRecordsAsync(_info.ContractGuid, connection, transaction);
                        await InsertNewSaldoRecordsAsync(_info, connection, transaction);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private IEnumerable<SaldoServiceInfoDto> GetMissingServices()
        {
            var list = _info.Services.ToList();
            foreach (var item in _emptyRecDictionary)
            {
                var service = _info.Services.SingleOrDefault(x => x.Index == item.Key);
                if (service == null)
                {
                    list.Add(new SaldoServiceInfoDto()
                    {
                        ServiceId = item.Value.ServiceId,
                        ServiceTitle = item.Value.ServiceTitle,
                        PayAccountId = item.Value.PayAccountId,
                        PayAccountTitle = item.Value.PayAccountTitle,
                        CurrencyId = 1,
                        Forbidden = 0,
                        Saldo = 0,
                        Summa = 0,
                        Index = item.Value.Index
                    });
                }
            }
            return list;
        }

        private async Task InsertNewSaldoRecordsAsync(SaldoInfoDto saldoInfo, IDbConnection connection, IDbTransaction transaction)
        {
            if (!saldoInfo.Orders.NullOrEmpty())
            {
                var sql = @"
                INSERT INTO dbo.SaldoOrders
                    (IdGuid,TeoGuid,OrderId,ContractGuid,AccountId,OrderTitle,BlockSum,MoveSum,ResultBlockSum,ServiceId)
                VALUES (@IdGuid,@TeoGuid,@OrderId,@ContractGuid,@AccountId,@OrderTitle,@BlockSum,@MoveSum,@ResultBlockSum,@ServiceId)";

                foreach (var order in saldoInfo.Orders)
                {
                    var args = new
                    {
                        IdGuid = Guid.NewGuid(),
                        ServiceId = order.ServiceId,
                        TeoGuid = order.RequestGuid,
                        OrderId = order.Id,
                        ContractGuid = saldoInfo.ContractGuid,
                        AccountId = order.AccountId,
                        OrderTitle = order.RequestTitle,
                        BlockSum = order.BlockSum,
                        MoveSum = order.MoveSum,
                        ResultBlockSum = order.ResultBlockSum
                    };
                    await connection.ExecuteAsync(sql, args, commandTimeout: COMMAND_TIMEOUT, transaction: transaction);
                }
            }

            if (saldoInfo.Services.NullOrEmpty())
            {
                saldoInfo.Services = GetEmptyServiceResult();
            }

            var sql2 = @"
                INSERT INTO dbo.SaldoServices
                    (IdGuid,ServiceId,ServiceTitle,ContractGuid,PayAccountId,PayAccountTitle,CurrencyId,Forbidden,Saldo,Summa,[Index])
                VALUES (@IdGuid,@ServiceId,@ServiceTitle,@ContractGuid,@PayAccountId,@PayAccountTitle,@CurrencyId,@Forbidden,@Saldo,@Summa,@Index)
                ";

            foreach (var service in saldoInfo.Services)
            {
                var args = new
                {
                    IdGuid = Guid.NewGuid(),
                    ServiceId = service.ServiceId,
                    ServiceTitle = service.ServiceTitle,
                    ContractGuid = saldoInfo.ContractGuid,
                    PayAccountId = service.PayAccountId,
                    PayAccountTitle = service.PayAccountTitle,
                    CurrencyId = service.CurrencyId,
                    Forbidden = service.Forbidden,
                    Saldo = service.Saldo,
                    Summa = service.Summa,
                    Index = service.Index
                };
                await connection.ExecuteAsync(sql2, args, commandTimeout: COMMAND_TIMEOUT, transaction: transaction);
            }
        }

        private IEnumerable<SaldoServiceInfoDto> GetEmptyServiceResult()
        {
            var mockList = new List<SaldoServiceInfoDto>();
            foreach (var item in _emptyRecDictionary)
            {
                mockList.Add(new SaldoServiceInfoDto()
                {
                    ServiceId = item.Value.ServiceId,
                    ServiceTitle = item.Value.ServiceTitle,
                    PayAccountId = item.Value.PayAccountId,
                    PayAccountTitle = item.Value.PayAccountTitle,
                    CurrencyId = 1,
                    Forbidden = 0,
                    Saldo = 0,
                    Summa = 0,
                    Index = item.Value.Index
                });
            }

            return mockList;
        }

        private async Task RemoveOldSaldoRecordsAsync(Guid guid, IDbConnection connection, IDbTransaction transaction)
        {
            var sql1 = @"DELETE FROM SaldoOrders WHERE ContractGuid = @ContractGuid";
            var sql2 = @"DELETE FROM SaldoServices WHERE ContractGuid = @ContractGuid";

            var args = new { ContractGuid = guid };
            await connection.ExecuteAsync(sql1, args, commandTimeout: COMMAND_TIMEOUT, transaction: transaction);
            await connection.ExecuteAsync(sql2, args, commandTimeout: COMMAND_TIMEOUT, transaction: transaction);
        }
    }
}
