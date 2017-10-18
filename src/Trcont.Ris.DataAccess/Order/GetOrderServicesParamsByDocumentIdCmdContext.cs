namespace Trcont.Ris.DataAccess.Order
{
    using System.Collections.Generic;

    public class GetOrderServicesParamsByDocumentIdCmdContext
    {
        public int OrderId { get; set; }

        public IEnumerable<int> ServiceIds { get; set; }
    }
}
