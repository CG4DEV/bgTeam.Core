namespace Trcont.Ris.DataAccess.Order
{
    using System.Collections.Generic;

    public class GetKPServicesParamsByDocumentIdCmdContext
    {
        public int OrderId { get; set; }

        public IEnumerable<int> ServiceIds { get; set; }
    }
}
