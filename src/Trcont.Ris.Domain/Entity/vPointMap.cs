namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class vPointMap
    {
        public Guid MasterCnsiGuid { get; set; }

        public Guid SlaveCnsiGuid { get; set; }

        public string SlaveCnsiCode { get; set; }
    }
}
