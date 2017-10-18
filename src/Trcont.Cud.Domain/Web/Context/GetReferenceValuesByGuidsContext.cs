namespace Trcont.Cud.Domain.Web.Context
{
    using System;
    using System.Collections.Generic;

    public class GetReferenceValuesByGuidsContext
    {
        public IEnumerable<Guid> ReferenceGuids { get; set; }
    }
}
