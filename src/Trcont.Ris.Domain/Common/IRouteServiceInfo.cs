namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Domain.Common;

    public interface IRouteServiceInfo : IServiceInfo
    {
        int ServiceFortId { get; set; }

        string FromPointCode { get; set; }

        string ToPointCode { get; set; }

        int FromPointId { get; set; }

        int ToPointId { get; set; }
    }
}
