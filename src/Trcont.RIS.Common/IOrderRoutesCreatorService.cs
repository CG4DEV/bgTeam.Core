namespace Trcont.RIS.Common
{
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;

    public interface IOrderRoutesCreatorService
    {
        IEnumerable<OrdersRoute> GetOrderRoutes(RouteServiceOrderDto order);
    }
}
