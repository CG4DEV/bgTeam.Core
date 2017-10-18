namespace Trcont.Ris.DataAccess.Order
{
    using System;

    public class GetOrdersByContractIdCmdContext
    {
        public GetOrdersByContractIdCmdContext()
        {
            PageOffset = 1;
            PageSize = 10;
        }

        public Guid ContractGuid { get; set; }

        public int PageOffset { get; set; }

        public int PageSize { get; set; }

        public string AddToWhereBlock { get; set; }

        public string AddToOrderByBlock { get; set; }

        public object WhereBlockParams { get; set; }
    }
}
