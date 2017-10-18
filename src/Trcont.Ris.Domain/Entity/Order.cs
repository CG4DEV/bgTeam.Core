namespace Trcont.Ris.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;
    using Trcont.Ris.Domain.Enums;

    [TableName("Orders")]
    public class Order : EntityBase
    {
        public Guid IrsGuid { get; set; }

        public int? TeoId { get; set; }

        public Guid? TeoGuid { get; set; }

        public string Number { get; set; }

        public int ClientId { get; set; }

        public int ContractId { get; set; }

        public DateTime OrderDate { get; set; }

        public int TrainTypeId { get; set; }

        public string DocumentTitle { get; set; }

        public OrderStatusEnum StatusId { get; set; }

        public ContrRelationsTypeEnum ContrRelationsTypeId { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public string PeriodBeginOffset { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public string PeriodEndOffset { get; set; }

        public float Weight { get; set; }

        public float WeightBrutto { get; set; }

        public int? EtsngId { get; set; }

        public int? GngId { get; set; }

        public int CurrencyId { get; set; }

        public int OutCategory { get; set; }

        public int SendTypeId { get; set; }

        public int? ContainerQuantity { get; set; }

        public int ContOwner { get; set; }

        public int WagonPark { get; set; }

        public int Speed { get; set; }

        public int CustomType { get; set; }

        public int PlaceFromId { get; set; }

        public int CountryFromId { get; set; }

        public int PlaceToId { get; set; }

        public int CountryToId { get; set; }

        public int? TransTypeId { get; set; }
    }
}
