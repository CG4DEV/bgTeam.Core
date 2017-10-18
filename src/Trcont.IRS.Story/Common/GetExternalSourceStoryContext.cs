namespace Trcont.IRS.Story.Common
{
    using System;
    using System.Collections.Generic;

    public class GetExternalSourceStoryContext
    {
        public int ExternalSourceId { get; set; }

        public IEnumerable<Guid> ExternalCodeGuids { get; set; }
    }
}
