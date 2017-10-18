namespace Trcont.Cud.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Dto;

    public interface IRoute
    {
        int Number { get; set; }

        string Name { get; set; }

        Guid? FromPointGUID { get; set; }

        string FromPointTitle { get; set; }

        string FromPointCode { get; set; }

        Guid? ToPointGUID { get; set; }

        string ToPointTitle { get; set; }

        string ToPointCode { get; set; }

        IList<RouteService> Services { get; set; }

        Guid? FromCountryGuid { get; set; }

        Guid? ToCountryGuid { get; set; }
    }
}
