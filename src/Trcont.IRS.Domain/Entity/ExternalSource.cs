namespace Trcont.IRS.Domain.Entity
{
    using System;

    public class ExternalSource
    {
        public int Ref2ExternalSourcesId { get; set; }

        public int ReferenceId { get; set; }

        public int ExternalSourceId { get; set; }

        public string ExternalCode1 { get; set; }

        public string ExternalCode2 { get; set; }

        public string ExternalCode3 { get; set; }

        public Guid? ExternalCodeGuid { get; set; }
    }
}
