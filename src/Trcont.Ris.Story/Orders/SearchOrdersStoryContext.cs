namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;

    public class SearchOrdersStoryContext
    {
        public SearchOrdersStoryContext()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public string OrderNumber { get; set; }

        public Guid ContractGuid { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<OrderValue> OrderBy { get; set; }
    }

    public class OrderValue
    {
        public string Field { get; set; }

        public bool Asc { get; set; } = true;
    }
}
