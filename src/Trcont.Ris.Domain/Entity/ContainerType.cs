namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class ContainerType : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string CNSICode { get; set; }
    }
}
