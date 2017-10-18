namespace Trcont.Ris.Domain.Dto
{
    using System;
    using Trcont.Ris.Domain.Entity;

    public class OrderServicesDto : OrderServices
    {
        public string ServiceCode { get; set; }

        public string ServiceTitle { get; set; }

        public Guid FromPointGuid { get; set; }

        public string FromPointTitle { get; set; }

        public string FromPointCnsiCode { get; set; }

        public Guid FromPointCnsiGuid { get; set; }

        public int? FromCountryId { get; set; }

        public Guid? FromCountryGuid { get; set; }

        public string FromCountryTitle { get; set; }

        public Guid ToPointGuid { get; set; }

        public Guid? ToCountryGuid { get; set; }

        public string ToPointTitle { get; set; }

        public string ToPointCnsiCode { get; set; }

        public Guid ToPointCnsiGuid { get; set; }

        public int? ToCountryId { get; set; }

        public string ToCountryTitle { get; set; }

        public string TerritoryTitle { get; set; }

        public string TerritoryCode { get; set; }

        public Guid? TerritoryGuid { get; set; }

        public int? CurrencyCode { get; set; }

        public string CurrencyTitle { get; set; }

        public Guid ServiceTypeGuid { get; set; }
    }
}
