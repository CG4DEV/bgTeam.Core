namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Service : EntityBase, IIrsDictionary
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Code { get; set; }

        public int ServiceGroupId { get; set; }
    }

    public enum ServiceGroupEnum
    {
        All = 663,

        EPU = 15358,

        ESU = 15359,
    }
}
