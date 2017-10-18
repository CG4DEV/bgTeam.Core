namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class vPointsDto : vPoints
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }

        public Guid CountryGuid { get; set; }
    }
}
