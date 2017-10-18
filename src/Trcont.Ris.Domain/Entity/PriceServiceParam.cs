namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PriceServiceParam : EntityBase
    {
        public int PriceServiceId { get; set; }

        public Guid ServiceParamGuid { get; set; }

        public string ValueText { get; set; }

        public Guid? ValueGuid { get; set; }

        public decimal? ValueNum { get; set; }

        public DateTime? ValueDate { get; set; }
    }
}
