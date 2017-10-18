namespace Trcont.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AutoRates
    {
        public int RowId { get; set; }

        public int PriceServiceId { get; set; }

        public decimal Rate { get; set; }

        public decimal RateVAT { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public Guid PointGUID { get; set; }

        public Guid ContainerTypeGUID { get; set; }

        public int? Duration { get; set; }

        public Guid ZoneGUID { get; set; }

        public int? ComplexPriceServiceId { get; set; }

        public decimal? ComplexRate { get; set; }

        public decimal WeightMin { get; set; }

        public decimal WeightMax { get; set; }

        public bool WeightMinIncluded { get; set; }

        public bool WeightMaxIncluded { get; set; }
    }
}
