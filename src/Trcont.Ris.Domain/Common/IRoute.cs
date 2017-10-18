namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;

    public interface IRoute : IBaseRoute
    {
        int Number { get; set; }

        string Name { get; set; }

        string FromPointTitle { get; set; }

        string FromPointCode { get; set; }

        string ToPointTitle { get; set; }

        string ToPointCode { get; set; }

        IList<RouteService> Services { get; set; }

        IEnumerable<OrderServiceParamsDto> ServiceParams { get; set; }

        Guid? FromCountryGuid { get; set; }

        Guid? ToCountryGuid { get; set; }

        int? FromCountryId { get; set; }

        string FromCountryTitle { get; set; }

        int? ToCountryId { get; set; }

        string ToCountryTitle { get; set; }
    }
}
