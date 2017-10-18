namespace Trcont.Ris.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Common;

    public interface IFssToIrsConvertor
    {
        string Convert(string xml, string routeId, string routeSpecId, IRouteServiceInfo[] services);

        Task<string> ConvertAsync(string xml, string routeId, string routeSpecId, IRouteServiceInfo[] services);
    }
}
