namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Unit : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string TitleSmall { get; set; }

        public int? Code { get; set; }
    }
}
