namespace Trcont.IRS.WebApp.Contexts
{
    using System;

    public class SptFSSDataConvertContext
    {
        public DateTime ReportDate { get; set; }

        public int CLCurrencyId { get; set; }

        public Guid FromCountryGUID { get; set; }

        public Guid ToCountryGUID { get; set; }

        public string ServiceXML { get; set; }

        public string USLXML { get; set; }
    }
}
