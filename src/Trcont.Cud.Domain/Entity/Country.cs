namespace Trcont.Cud.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Country
    {
        public Guid CountryGuid { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
