namespace Trcont.Ris.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Trcont.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Story.Contracts;
    using Trcont.Ris.Story.Orders;

    public partial class InfoController : Controller
    {
        [HttpPost]
        public async Task<ContractInfoDto> GetContractInfoByOrderGuid(GetContractInfoByOrderGuidStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<ContractInfoDto>();
        }

        [HttpPost]
        public async Task<OrderIds> SaveOrder(SaveOrUpdateOrderStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<OrderIds>();
        }

        [HttpPost]
        public async Task<OrderInfoDto> GetOrderInfoById(GetOrderInfoByIdStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<OrderInfoDto>();
        }

        [HttpPost]
        public async Task<TransPicDto> GetTransPicByOrderId(GetOrderTransPicStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<TransPicDto>();
        }

        [HttpPost]
        public async Task<Documents> CreateOrderBillById(CreateOrderBillByIdStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<Documents>();
        }

        [HttpPost]
        public async Task<PageDto<OrderDto>> SearchOrders(SearchOrdersStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<PageDto<OrderDto>>();
        }

        [HttpPost]
        public async Task<PageDto<OrderDto>> SearchArchiveOrders(SearchArchiveOrdersStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<PageDto<OrderDto>>();
        }

        [HttpPost]
        public async Task<OrdersCount> GetOrdersCount(GetOrdersCountStoryContext context)
        {
            return await _storyBuilder
                .Build(context)
                .ReturnAsync<OrdersCount>();
        }
    }
}
