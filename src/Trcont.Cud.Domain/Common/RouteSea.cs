namespace Trcont.Cud.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Dto;

    public class RouteSea : IRoute
    {
        public int Number { get; set; }

        public string Name { get; set; }


        public Guid? FromPointGUID { get; set; }

        public string FromPointTitle { get; set; }

        public string FromPointCode { get; set; }

        public Guid? ToPointGUID { get; set; }

        public string ToPointTitle { get; set; }

        public string ToPointCode { get; set; }

        public IEnumerable<KPServicesDto> ServicesExt { get; set; }

        public IList<RouteService> Services { get; set; }

        public Guid? FromCountryGuid { get; set; }

        public Guid? ToCountryGuid { get; set; }
    }
}
