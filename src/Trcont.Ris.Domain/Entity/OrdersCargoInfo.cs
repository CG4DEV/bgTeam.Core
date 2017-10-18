namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class OrdersCargoInfo
    {
        public int OrderId { get; set; }

        public string ETSNGCode { get; set; }

        //public Guid ETSNGGuid { get; set; }

        public string GNGCode { get; set; }

        //public Guid? GNGGuid { get; set; }

        public OrdersCargoInfo()
        {
        }

        public OrdersCargoInfo(string etsngCode)
        {
            ETSNGCode = etsngCode;
        }

        public OrdersCargoInfo(string etsngCode, string gngCode)
        {
            ETSNGCode = etsngCode;
            GNGCode = gngCode;
        }
    }
}
