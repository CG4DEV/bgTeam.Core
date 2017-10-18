namespace Trcont.Cud.Domain.Web.Dto
{
    using System;

    public class FactInfo
    {
        public int FactTransId { get; set; }

        public DateTime ReportDate { get; set; }

        public string KontNumber { get; set; }

        public string WagonNumber { get; set; }

        public DateTime OutDate { get; set; }

        public DateTime ComDate { get; set; }

        public int FactSourceId { get; set; }

        public string NaklNumber { get; set; }
    }
}
