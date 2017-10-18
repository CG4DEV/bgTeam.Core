namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class Zone : EntityBaseIdentity, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Name { get; set; }

        public string CnsiCode { get; set; }
    }
}
