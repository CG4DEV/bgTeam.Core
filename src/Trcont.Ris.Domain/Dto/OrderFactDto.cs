namespace Trcont.Ris.Domain.Dto
{
    using System;
    using bgTeam.Extensions;
    using Trcont.Ris.Domain.Enums;

    public class OrderFactDto
    {
        public int OrderId { get; set; }

        // ex. OrderId
        public int TeoId { get; set; }

        public string OrderNumber { get; set; }

        public string ContNumber { get; set; }

        public string NaklNumber { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public string WagonNumber { get; set; }

        public int? TransStationFromId { get; set; }

        public string StationFromName { get; set; }

        public int? TransStationToId { get; set; }

        public string StationToName { get; set; }

        public FactSourceEnum FactSource { get; set; }

        public string FactSourceName => FactSource.GetDescription();

        public string AutoOut { get; set; }

        public string AutoTo { get; set; }
    }
}
