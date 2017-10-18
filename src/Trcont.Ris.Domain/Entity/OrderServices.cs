namespace Trcont.Ris.Domain.Entity
{
    public class OrderServices
    {
        public int ServiceId { get; set; }

        public int OrderId { get; set; }

        public int ServiceTypeId { get; set; }

        public int? FromPointId { get; set; }

        public int? ToPointId { get; set; }

        public int? TerritoryId { get; set; }

        public int? CurrencyId { get; set; }

        public decimal Tariff { get; set; }

        public decimal TariffVAT { get; set; }

        public decimal Summ { get; set; }

        public decimal SummVAT { get; set; }

        public int TariffType { get; set; }

        public int ContractId { get; set; }

        public bool IsActive { get; set; }

        public int ArmIndex { get; set; }

        public decimal? SrcVolume { get; set; }
    }
}
