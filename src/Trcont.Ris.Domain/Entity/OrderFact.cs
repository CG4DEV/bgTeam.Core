namespace Trcont.Ris.Domain.Entity
{
    using System;
    using Trcont.Ris.Domain.Enums;

    public class OrderFact
    {
        public long Id { get; set; }

        public int OrderId { get; set; }

        public string ContNumber { get; set; }

        public string NaklNumber { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public DateTime FactAccessDate { get; set; }

        public string WagonNumber { get; set; }

        public FactSourceEnum FactSourceId { get; set; }

        public DateTime? ReportDate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime TimeStamp { get; set; }

        public int? TransStationFromId { get; set; }

        public int? TransStationToId { get; set; }
    }
}
