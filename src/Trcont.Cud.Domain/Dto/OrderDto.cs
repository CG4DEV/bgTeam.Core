namespace Trcont.Cud.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class OrderDto
    {
        public Guid ReferenceGuid { get; set; }

        public Guid? RefDocumentGuid { get; set; }

        public DateTime CreateDate { get; set; }

        public int Number { get; set; }

        public int Status { get; set; }

        public decimal Summ { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public Guid ClientGuid { get; set; }

        public string ClientName { get; set; }

        public string ContractNumber { get; set; }

        public Guid PlaceFromGuid { get; set; }

        public Guid PlaceToGuid { get; set; }

        public Guid ETSNGGuid { get; set; }

        public Guid ContainerTypeGuid { get; set; }

        public Guid? ContainerTypeCNSIGuid { get; set; }

        public DateTime? LoadDate { get; set; }

        public bool IsTeo { get; set; }

        public string FileLink { get; set; }

        public Guid? PlaceFromCNSIGuid { get; set; }

        public Guid? PlaceToCNSIGuid { get; set; }

        public Guid? EtsngCNSIGuid { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public DateTime? SendDate { get; set; }


        public string PlaceFromTitle { get; set; }

        public string PlaceToTitle { get; set; }

        public string ETSNGTitle { get; set; }

        public string ContainerTypeTitle { get; set; }

        public string StatusTitle { get; set; }


        public IEnumerable<DateTime> PlanDates { get; set; }
    }
}
