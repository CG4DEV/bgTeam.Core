namespace Trcont.Ris.Domain.Dto
{
    using System.Collections.Generic;

    public class OrdersCount
    {
        public int ActiveCount { get; set; }

        public int ArchiveCount { get; set; }

        public IEnumerable<OrdersCountByMonthYear> ArchiveMonthYearCount { get; set; }
    }

    public class OrdersCountByMonthYear
    {
        public int Month { get; set; }

        public int Year { get; set; }

        public int OrdersCount { get; set; }
    }
}
