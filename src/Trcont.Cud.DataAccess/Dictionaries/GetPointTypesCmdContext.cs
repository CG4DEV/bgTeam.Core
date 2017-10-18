namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;
    using System.Collections.Generic;

    public class GetPointTypesCmdContext
    {
        public IEnumerable<Guid> PlaceGuids { get; set; }
    }
}
