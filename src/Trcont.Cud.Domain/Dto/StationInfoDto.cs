namespace Trcont.Cud.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain.Entity;

    public class StationInfoDto : Station
    {
        public bool IsAutoDelivery { get; set; }

        public string ZoneStr { get; set; }

        public string ZoneTitle { get; set; }

        public string ZoneGuid { get; set; }
    }
}
