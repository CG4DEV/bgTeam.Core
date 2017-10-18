namespace Trcont.Cud.Story.Common
{
    using System;

    public class GetOrdersByContractGuidStoryContext
    {
        public GetOrdersByContractGuidStoryContext()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public Guid ContractGuid { get; set; }

        public string ServerUrl { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}