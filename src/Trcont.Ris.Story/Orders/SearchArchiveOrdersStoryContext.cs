namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SearchArchiveOrdersStoryContext : SearchOrdersStoryContext
    {
        public int Month { get; set; }

        public int Year { get; set; }
    }
}
