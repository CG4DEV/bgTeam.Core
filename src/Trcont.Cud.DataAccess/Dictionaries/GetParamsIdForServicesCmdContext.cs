namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;
    using System.Collections.Generic;

    public class GetParamsIdForServicesCmdContext
    {
        public IEnumerable<Guid> ServiceGuids { get; set; }
    }
}