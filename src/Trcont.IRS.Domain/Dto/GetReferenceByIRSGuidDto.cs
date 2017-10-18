namespace Trcont.IRS.Domain.Dto
{
    using System;

    public class GetReferenceByIRSGuidDto
    {
        public Guid ReferenceGuid { get; set; }

        public string Value { get; set; }

        public string ReferenceAccount { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
