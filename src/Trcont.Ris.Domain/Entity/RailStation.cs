namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class RailStation : EntityBase
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Account { get; set; }

        public string CnsiCode { get; set; }

        public Guid CnsiGuid { get; set; }

        public int? RailRoadId { get; set; }

        public int CountryId { get; set; }

        public int? Code { get; set; }

        public string Paragraphs { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
