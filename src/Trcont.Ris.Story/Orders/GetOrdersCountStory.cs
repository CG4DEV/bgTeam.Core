namespace Trcont.Ris.Story.Orders
{
    using bgTeam;
    using bgTeam.DataAccess;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Dto;

    public class GetOrdersCountStory : IStory<GetOrdersCountStoryContext, OrdersCount>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public GetOrdersCountStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public OrdersCount Execute(GetOrdersCountStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrdersCount> ExecuteAsync(GetOrdersCountStoryContext context)
        {
            OrdersCount result = await GetOrdersCount(context);
            result.ArchiveMonthYearCount = await GetMonthYearOrdersCount(context);
            return result;
        }

        private async Task<IEnumerable<OrdersCountByMonthYear>> GetMonthYearOrdersCount(GetOrdersCountStoryContext context)
        {
            var sql = @"
                SELECT
                  YEAR(o.OrderDate) AS Year,
                  MONTH(o.OrderDate) AS Month,
                  COUNT(1) AS OrdersCount
                FROM Orders o WITH(NOLOCK)
                    INNER JOIN Contract ct WITH(NOLOCK) ON o.ContractId = ct.Id
                WHERE ct.IrsGuid = @ContractGuid AND ((ct.EndDate > GETDATE() AND o.StatusId IN (3)) OR (ct.EndDate < GETDATE()))
                    AND o.TeoId IS NOT NULL
                GROUP BY YEAR(o.OrderDate), MONTH(o.OrderDate)
            ";

            return await _repository.GetAllAsync<OrdersCountByMonthYear>(sql, new
            {
                ContractGuid = context.ContractGuid
            });
        }

        private async Task<OrdersCount> GetOrdersCount(GetOrdersCountStoryContext context)
        {
            var sql = @"
                SELECT
                  SUM(tmp.IsActive) AS ActiveCount,
                  SUM(tmp.IsArchive) AS ArchiveCount
                FROM
                (
                  SELECT
                    CASE
                      WHEN (ct.EndDate IS NULL OR ct.EndDate > GETDATE()) AND o.StatusId IN (0,2)
                      THEN 1
                      ELSE 0
                    END IsActive,
                    CASE
                      WHEN ((ct.EndDate > GETDATE() AND o.StatusId IN (3)) OR (ct.EndDate < GETDATE()))
                      THEN 1
                      ELSE 0
                    END IsArchive
                  FROM Orders o WITH(NOLOCK)
                    INNER JOIN Contract ct WITH(NOLOCK) ON o.ContractId = ct.Id
                  WHERE ct.IrsGuid = @ContractGuid AND o.TeoId IS NOT NULL
                ) tmp
            ";

            return await _repository.GetAsync<OrdersCount>(sql, new
            {
                ContractGuid = context.ContractGuid
            });
        }
    }
}
