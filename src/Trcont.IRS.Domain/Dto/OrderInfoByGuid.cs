namespace Trcont.IRS.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class OrderInfoByGuid
    {
        public int Id { get; set; }

        public Guid IrsGuid { get; set; }

        public int TeoId { get; set; }

        public Guid TeoGuid { get; set; }

        public string Number { get; set; }

        public DateTime OrderDate { get; set; }

        public IEnumerable<RisKPServiceInfo> KpServices { get; set; }

        public IEnumerable<RisTeoServiceInfo> TeoServices { get; set; }

        public IEnumerable<KpServiceParamInfo> KpServiceParams { get; set; }

        public IEnumerable<TeoServiceParamInfo> TeoServiceParams { get; set; }
    }

    public class RisKPServiceInfo
    {
        public int ServiceId { get; set; }

        public int OrderId { get; set; }

        public int ServiceTypeId { get; set; }

        public int? FromPointId { get; set; }

        public int? ToPointId { get; set; }

        public int? CurrencyId { get; set; }

        public decimal? Tariff { get; set; }

        public decimal? TariffVAT { get; set; }

        public decimal? Summ { get; set; }

        public decimal? SummVAT { get; set; }

        public int TariffType { get; set; }

        public int? IsActive { get; set; }

        public int? ArmIndex { get; set; }

        public int? SourceReferenceId { get; set; }

        public int? SourcePriceServiceId { get; set; }

        public decimal? SrcVolume { get; set; }
    }

    public class RisTeoServiceInfo
    {
        public int ServiceId { get; set; }

        public int? TeoId { get; set; }

        public int? OrderId { get; set; }

        public int ServiceTypeId { get; set; }

        public int? FromPointId { get; set; }

        public int? ToPointId { get; set; }

        public int? TerritoryId { get; set; }

        public int? ParentTeoServiceId { get; set; }

        public int? CurrencyId { get; set; }

        public decimal? Tariff { get; set; }

        public decimal? TariffVAT { get; set; }

        public decimal? Summ { get; set; }

        public decimal? SummVAT { get; set; }

        public int TariffType { get; set; }

        public int? ContractId { get; set; }

        public int ArmIndex { get; set; }

        public int? SourceReferenceId { get; set; }

        public int? SourcePriceServiceId { get; set; }

        public decimal? SrcVolume { get; set; }
    }

    public class KpServiceParamInfo
    {
        public long Id { get; set; }

        public long? OrderServiceId { get; set; }

        public long? OrderId { get; set; }

        public long? TeoId { get; set; }

        public Guid? AttribGUID { get; set; }

        public Guid? OrderServiceGUID { get; set; }

        public string AttribValueRus { get; set; }

        public decimal? AttribNumValue { get; set; }

        public DateTime? AttribDateValue { get; set; }
    }

    public class TeoServiceParamInfo
    {
        public long Id { get; set; }

        public long? OrderId { get; set; }

        public long? TeoId { get; set; }

        public Guid? AttribGUID { get; set; }

        public long? TeoServiceId { get; set; }

        public Guid? TeoServiceGUID { get; set; }

        public string AttribValueRus { get; set; }

        public decimal? AttribNumValue { get; set; }

        public DateTime? AttribDateValue { get; set; }
    }
}
