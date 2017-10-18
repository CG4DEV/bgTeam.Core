namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PriceService : EntityBase
    {
        //public int Id { get; set; }

        public int PriceId { get; set; }

        public int ServiceId { get; set; }

        public int UnitId { get; set; }

        public int CurrencyId { get; set; }

        public decimal Rate { get; set; }

        public decimal RateVAT { get; set; }

        public int? Duration { get; set; }

        public int TarifType { get; set; }

        public decimal? WeightRateBorder { get; set; }

        public decimal? WeightStepIncrement { get; set; }

        public decimal? RateStepIncrement { get; set; }
    }
}
