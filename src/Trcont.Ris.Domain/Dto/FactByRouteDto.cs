namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class FactByRouteDto
    {
        public int OrderId { get; set; }

        public string ContNumber { get; set; }

        public string NaklNumber { get; set; }

        public DateTime? TransDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public Guid FromPointGuid { get; set; }

        public Guid ToPointGuid { get; set; }
    }
}
