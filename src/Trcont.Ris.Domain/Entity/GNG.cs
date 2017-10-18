namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class GNG : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }
    }
}
