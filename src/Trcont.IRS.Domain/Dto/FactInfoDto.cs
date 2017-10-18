namespace Trcont.IRS.Domain.Dto
{
    using System;
    using Trcont.IRS.Domain.Entity;

    public class FactInfoDto : FactTransport
    {
        public Guid ReferenceGuid { get; set; }
    }
}
