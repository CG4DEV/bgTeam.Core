namespace Trcont.Ris.DataAccess.Order
{
    using bgTeam.DataAccess;
    using DapperExtensions.Mapper;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Common.Utils;
    using Trcont.Ris.DataAccess.Helpers;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;

    public class GetKPServicesParamsByDocumentIdCmd : ICommand<GetKPServicesParamsByDocumentIdCmdContext, IEnumerable<OrderServiceParamsDto>>
    {
        private readonly IRepository _repository;

        public GetKPServicesParamsByDocumentIdCmd(IRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<OrderServiceParamsDto> Execute(GetKPServicesParamsByDocumentIdCmdContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<IEnumerable<OrderServiceParamsDto>> ExecuteAsync(GetKPServicesParamsByDocumentIdCmdContext context)
        {
            var result = await _repository.GetAllAsync<OrderServiceParamsDto>(
               @"SELECT kp.Id
                    ,kp.TeoId
                    ,kp.OrderId
                    ,kp.OrderServiceId
                    ,kp.OrderServiceGuid
                    ,kp.AttribGuid
                    ,sp.NameRus AS AttribName
                    ,kp.AttribValueRus
                    ,kp.AttribNumValue
                    ,kp.AttribDateValue
                    ,kp.CreateDate
                    ,kp.TimeStamp
                FROM KPServiceParams kp
                  LEFT JOIN ServiceParams sp ON sp.ParamGuid = kp.AttribGuid AND sp.IsEditUserValue = 1
                  WHERE kp.OrderId = @OrderId AND kp.OrderServiceId IN (SELECT Id FROM @ServiceIds)",
               new { OrderId = context.OrderId, ServiceIds = new IntDbType(context.ServiceIds) });

            return result.TrimAttribName();
        }
    }
}
