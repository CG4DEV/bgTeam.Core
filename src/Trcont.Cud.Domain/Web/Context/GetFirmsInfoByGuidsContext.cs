namespace Trcont.Cud.Domain.Web.Context
{
    using System;
    using System.Collections.Generic;

    public class GetFirmsInfoByGuidsContext
    {
        public IEnumerable<Guid> ClientGuids { get; set; }
    }
}
