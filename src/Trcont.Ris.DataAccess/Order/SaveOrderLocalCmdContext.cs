namespace Trcont.Ris.DataAccess.Order
{
    using System.Collections.Generic;
    using Trcont.App.Service.Entity;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public class SaveOrderLocalCmdContext
    {
        public OrderInfo Order { get; set; }

        public OrderInfoByGuid OrderInfo { get; set; }

        public IEnumerable<RouteForSave> OrderRoutes { get; set; }
        public IEnumerable<OrdersCargoInfo> OrderCargo { get; set; }
    }
}
