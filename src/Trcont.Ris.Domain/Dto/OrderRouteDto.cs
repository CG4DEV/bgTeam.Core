namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public class OrderRouteDto : OrderRoute, IBaseRoute
    {
        public Guid? FromPointGUID { get; set; }

        public Guid? ToPointGUID { get; set; }

        public int? FromCountryId { get; set; }

        public string FromCountryTitle { get; set; }

        public int? ToCountryId { get; set; }

        public string ToCountryTitle { get; set; }

        public Guid? FromCountryGuid { get; set; }

        public Guid? ToCountryGuid { get; set; }

        public RouteTypeEnum RouteType { get; set; }

        public IEnumerable<RouteService> Services { get; set; }

        public IEnumerable<OrderServiceParamsDto> ServiceParams { get; set; }
    }
}
