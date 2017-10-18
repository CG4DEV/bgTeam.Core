namespace Trcont.Cud.Domain.Web.Context
{
    using System;
    using System.Collections.Generic;

    public class GetIrsGuidByCNSIGuidContext
    {
        public IEnumerable<GuidsGroup> Groups { get; set; }
    }

    public class GuidsGroup
    {
        public IEnumerable<int> RefGroupIds { get; set; }

        public IEnumerable<Guid> CNSIGuids { get; set; }
    }
}
