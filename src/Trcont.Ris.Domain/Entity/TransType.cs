namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TransType : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
