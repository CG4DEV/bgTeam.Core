namespace Trcont.Domain.OtmData
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Заказ для интеграции с ОТМ
    /// </summary>
    public class OtmRelease
    {
        public string OrderId { get; set; }

        public string OrderName { get; set; }

        public List<string> Routes { get; set; }

        public string StatusType { get; set; }

        public string StatusName { get; set; }

        public DateTime DateLoad { get; set; }

        public DateTime DateArrival { get; set; }

        public string StationFrom { get; set; }

        public string StationTo { get; set; }

        public IEnumerable<OtmCargo> CargoList { get; set; }
    }
    
    public class OtmCargo
    {
        public string EstngCode { get; set; }

        public string ContainerType { get; set; }

        public float WeightBrutto { get; set; }

        public float WeightNet { get; set; }

        /// <summary>
        /// Default: MTON
        /// </summary>
        public string WeightUom { get; set; } = "MTON";

        public float Volume { get; set; }

        /// <summary>
        /// Default: CUMTR
        /// </summary>
        public string VolumeUom { get; set; } = "CUMTR";
    }
}
