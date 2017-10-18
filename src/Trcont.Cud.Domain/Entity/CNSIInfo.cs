namespace Trcont.Cud.Domain.Entity
{
    using System;

    public class CNSIInfo
    {
        public Guid ReferenceGuid { get; set; }

        public string ExternalCode { get; set; }

        public string ReferenceTitle { get; set; }

        public int ExternalSourceId { get; set; }

        public string ReferenceAccount { get; set; }

        public int ReferenceId { get; set; }
    }
}
