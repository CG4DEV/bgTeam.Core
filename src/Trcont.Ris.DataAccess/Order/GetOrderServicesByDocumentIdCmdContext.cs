namespace Trcont.Ris.DataAccess.Order
{
    using System.Collections.Generic;

    public class GetOrderServicesByDocumentIdCmdContext
    {
        public int OrderId { get; set; }

        /// <summary>
        /// Скрыть дочерние сервисы, применимо для OrdersService. По умолчанию true
        /// </summary>
        public bool SkipChildren { get; set; } = true;
    }
}
