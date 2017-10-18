namespace Trcont.Cud.DataAccess.Dictionaries
{
    using System;
    using System.Collections.Generic;

    public class GetCNSIInfoByGuidsCmdContext
    {
        public GetCNSIInfoByGuidsCmdContext(IEnumerable<Guid?> guids)
        {
            Guids = guids;
        }

        public IEnumerable<Guid?> Guids { get; set; }
    }
}
