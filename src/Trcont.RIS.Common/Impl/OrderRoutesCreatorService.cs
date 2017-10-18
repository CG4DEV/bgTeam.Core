namespace Trcont.RIS.Common.Impl
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;
    using Trcont.Ris.Domain.TransPicture;

    public class OrderRoutesCreatorService : IOrderRoutesCreatorService
    {
        public IEnumerable<OrdersRoute> GetOrderRoutes(RouteServiceOrderDto order)
        {
            order.CheckNull(nameof(order));

            if (order.Services.NullOrEmpty())
            {
                return Enumerable.Empty<OrdersRoute>();
            }

            return order.Services
                .OrderBy(x => x.ArmIndex)
                .Select(x => new OrdersRoute()
                {
                    TeoId = x.TeoId,
                    OrderId = x.OrderId,
                    FromPointId = x.FromPointId,
                    FromPointCode = x.FromPointCode,
                    FromPointTitle = x.FromPointTitle,
                    ToPointId = x.ToPointId,
                    ToPointCode = x.ToPointCode,
                    ToPointTitle = x.ToPointTitle,
                    ArmIndex = x.ArmIndex,
                    CreateDate = DateTime.Now,
                    TimeStamp = DateTime.Now,
                    RouteType = GetRouteType(x.FromPointType, x.ToPointType)
                })
                .DistinctBy(x => x.ArmIndex)
                .ToArray();
        }

        private RouteTypeEnum GetRouteType(PointTypeEnum fromPointType, PointTypeEnum toPointType)
        {
            if (fromPointType == PointTypeEnum.PointPortType &&
                toPointType == PointTypeEnum.PointPortType)
            {
                return RouteTypeEnum.VESSEL;
            }

            return RouteTypeEnum.RAIL;
        }
    }
}
