namespace Trcont.IRS.Domain.Dto
{
    using System;

    public class IrsGuidAndCNSIGuid
    {
        public Guid CNSIGuid { get; set; }

        public Guid ReferenceGuid { get; set; }

        public string Title { get; set; }

        public string Account { get; set; }
    }
}
