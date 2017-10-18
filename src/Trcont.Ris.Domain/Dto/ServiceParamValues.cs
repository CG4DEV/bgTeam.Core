namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class ServiceParamValues
    {
        public string Name { get; set; }

        public Guid IrsGuid { get; set; }

        public string Value { get; set; }
    }
}
