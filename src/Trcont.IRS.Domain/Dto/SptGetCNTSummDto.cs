namespace Trcont.IRS.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class SptGetCNTSummDto
    {
        public IEnumerable<SptGetCNTSummOrdersInfo> Orders { get; set; }

        public IEnumerable<SptGetCNTSummServicesInfo> Services { get; set; }
    }

    public class SptGetCNTSummServicesInfo
    {
        public int ServiceId { get; set; }

        public string ServiceTitle { get; set; }

        public int PayAccountId { get; set; }

        public string PayAccountTitle { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyTitle { get; set; }

        public int Forbidden { get; set; }

        public decimal Saldo { get; set; }

        public decimal Summa { get; set; }

        public int Index { get; set; }
    }

    public class SptGetCNTSummOrdersInfo
    {
        public int ServiceId { get; set; }

        public int AccountId { get; set; }

        public Guid RequestGuid { get; set; }

        public string RequestTitle { get; set; }

        public decimal BlockSum { get; set; }

        public decimal MoveSum { get; set; }

        public decimal ResultBlockSum { get; set; }
    }
}
