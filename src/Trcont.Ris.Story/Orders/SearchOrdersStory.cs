namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using Trcont.Common.Utils;
    using Trcont.Domain.Common;
    using Trcont.Ris.DataAccess.Order;
    using Trcont.Ris.Domain.Dto;

    public class SearchOrdersStory : IStory<SearchOrdersStoryContext, PageDto<OrderDto>>
    {
        private readonly string _orderNumberFilter = "AND o.Number LIKE @OrderNumber";

        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public SearchOrdersStory(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public PageDto<OrderDto> Execute(SearchOrdersStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<PageDto<OrderDto>> ExecuteAsync(SearchOrdersStoryContext context)
        {
            if (context.ContractGuid == Guid.Empty)
            {
                throw new ArgumentException("Неверные входные параметры", nameof(context.ContractGuid));
            }

            if (context.OrderBy != null)
            {
                GetOrdersByContractIdCmd.CheckOrderBy(context.OrderBy.Select(x => x.Field));
            }

            var blockWhere = "(ct.EndDate IS NULL OR ct.EndDate > GETDATE()) AND o.StatusId IN (0,2) AND o.TeoId IS NOT NULL";

            if (!string.IsNullOrWhiteSpace(context.OrderNumber))
            {
                blockWhere += $" {_orderNumberFilter}";
            }

            var prms = new
            {
                OrderNumber = $"%{context.OrderNumber}%"
            };

            var command = new GetOrdersByContractIdCmd(_repository);
            var cmdContext = new GetOrdersByContractIdCmdContext()
            {
                ContractGuid = context.ContractGuid,
                PageSize = context.PageSize,
                PageOffset = PagingHelper.GetOffsetByPageNumber(context.PageNumber, context.PageSize),
                WhereBlockParams = prms,
                AddToWhereBlock = blockWhere,
                AddToOrderByBlock = GetOrderByString(context.OrderBy)
            };

            var orders = await command.ExecuteAsync(cmdContext);

            var from = command.FromBlock;
            var where = command.WhereBlock;
            var cmdParams = command.GetQueryParams(cmdContext);

            //var totalCount = await GetOrdersCountByContractIdAsync(from, where, cmdParams);
            var matchCount = await GetOrdersCountAsync(from, where, cmdParams);

            return new PageDto<OrderDto>()
            {
                Page = orders,
                TotalCount = matchCount,
                MatchCount = matchCount
            };
        }

        private async Task<int> GetOrdersCountAsync(string from, string where, Dictionary<string, object> prms)
        {
            var sql =
                $@"SELECT COUNT(1)
                    {from}
                    {where}";

            return await _repository.GetAsync<int>(sql, prms);
        }

        private async Task<int> GetOrdersCountByContractIdAsync(string from, string where, Dictionary<string, object> prms)
        {
            var sql =
                $@"SELECT COUNT(1)
                    {from}
                    {where}";
            sql = sql.Replace(_orderNumberFilter, string.Empty);
            return await _repository.GetAsync<int>(sql, prms);
        }

        private string GetOrderByString(IEnumerable<OrderValue> orderBy)
        {
            if (orderBy != null && orderBy.Any())
            {
                return $"ORDER BY {string.Join(", ", orderBy.Select(x => $"{x.Field} {(x.Asc ? "ASC" : "DESC")}"))}";
            }

            return "ORDER BY OrderDate DESC";
        }
    }
}
