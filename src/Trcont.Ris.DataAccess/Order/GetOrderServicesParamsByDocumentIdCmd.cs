namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.Ris.DataAccess.Helpers;
    using Trcont.Ris.Domain.Common;

    public class GetOrderServicesParamsByDocumentIdCmd : ICommand<GetOrderServicesParamsByDocumentIdCmdContext, IEnumerable<OrderServiceParamsDto>>
    {
        private readonly IRepository _repository;

        public GetOrderServicesParamsByDocumentIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<OrderServiceParamsDto> Execute(GetOrderServicesParamsByDocumentIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<OrderServiceParamsDto>> ExecuteAsync(GetOrderServicesParamsByDocumentIdCmdContext context)
        {
            if (context.ServiceIds == null || !context.ServiceIds.Any())
            {
                return Enumerable.Empty<OrderServiceParamsDto>();
            }

            var result = await _repository.GetAllAsync<OrderServiceParamsDto>(
               @"SELECT osp.Id
                    ,osp.TeoId
                    ,osp.OrderId
                    ,osp.TeoServiceId
                    ,osp.TeoServiceGuid
                    ,osp.AttribGuid
                    ,sp.NameRus AS AttribName
                    ,osp.AttribValueRus
                    ,osp.AttribNumValue
                    ,osp.AttribDateValue
                    ,osp.CreateDate
                    ,osp.TimeStamp
                FROM OrdersServiceParams osp
                  INNER JOIN ServiceParams sp ON sp.ParamGuid = osp.AttribGuid AND sp.IsEditUserValue = 1
                  WHERE osp.OrderId = @OrderId AND osp.TeoServiceId IN (SELECT Id FROM @ServiceIds)",
               new { OrderId = context.OrderId, ServiceIds = new IntDbType(context.ServiceIds) });

            return result.TrimAttribName();
        }
    }
}
