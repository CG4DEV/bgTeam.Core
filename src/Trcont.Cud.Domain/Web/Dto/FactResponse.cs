namespace Trcont.Cud.Domain.Web.Dto
{
    using System;
    using System.Collections.Generic;

    public class FactResponse
    {
        public Guid ReferenceGuid { get; set; }

        public IEnumerable<FactInfo> FactTransportCollection { get; set; }
    }
}
