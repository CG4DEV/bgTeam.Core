namespace Trcont.IRS.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using Trcont.IRS.Domain.Entity;

    public class FactTransportByTeoGuidDto
    {
        public Guid ReferenceGuid { get; set; }

        public IEnumerable<FactTransport> FactTransportCollection { get; set; }
    }
}
