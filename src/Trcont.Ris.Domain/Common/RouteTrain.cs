namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;

    public class RouteTrain : IRoute
    {
        public int Number { get; set; }

        public string Name { get; set; }

        public Guid? FromPointGUID { get; set; }

        public string FromPointTitle { get; set; }

        public string FromPointCode { get; set; }

        public Guid? ToPointGUID { get; set; }

        public string ToPointTitle { get; set; }

        public string ToPointCode { get; set; }

        public IList<RouteService> Services { get; set; }

        public IEnumerable<OrderServiceParamsDto> ServiceParams { get; set; }

        public int? FromCountryId { get; set; }

        public string FromCountryTitle { get; set; }

        public int? ToCountryId { get; set; }

        public string ToCountryTitle { get; set; }

        public Guid? FromCountryGuid { get; set; }

        public Guid? ToCountryGuid { get; set; }

        public int ArmIndex { get; set; }
    }
}
