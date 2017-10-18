namespace Trcont.Ris.DataAccess.Common
{
    using Trcont.Ris.Domain.Entity;

    public class SaveDocumentByEntityIdCmdContext
    {
        public int EntityId { get; set; }

        /// <summary>
        /// 1 - Order, 2 - Contract
        /// </summary>
        public int EntityType { get; set; }

        public Documents Document { get; set; }
    }
}
