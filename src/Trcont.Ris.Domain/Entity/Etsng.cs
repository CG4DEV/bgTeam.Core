namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class Etsng : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }
    }
}
