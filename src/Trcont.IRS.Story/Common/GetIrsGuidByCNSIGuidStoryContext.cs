namespace Trcont.IRS.Story.Common
{
    using System;
    using System.Collections.Generic;

    public class GetIrsGuidByCNSIGuidStoryContext
    {
        public IEnumerable<GuidsGroup> Groups { get; set; }
    }

    public class GuidsGroup
    {
        public IEnumerable<int> RefGroupIds { get; set; }

        public IEnumerable<Guid> CNSIGuids { get; set; }
    }
}
