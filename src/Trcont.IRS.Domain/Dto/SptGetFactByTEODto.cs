namespace Trcont.IRS.Domain.Dto
{
    using System;

    public class SptGetFactByTEODto
    {
        public int FactTransId { get; set; }

        public DateTime ReportDate { get; set; }

        public string KontNumber { get; set; }

        public string WagonNumber { get; set; }
    }
}
