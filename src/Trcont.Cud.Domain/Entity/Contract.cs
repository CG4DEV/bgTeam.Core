namespace Trcont.Cud.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Contract
    {
        public Guid ContractGUID { get; set; }

        public string ContractNumber { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
