namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class OrderServiceAttribute
    {
        public int AttId { get; set; }

        public Guid AttGuid { get; set; }

        public int AttType { get; set; }

        public string Value { get; set; }

        public Guid? PlaceRender { get; set; }

        public bool IsRequired { get; set; }
    }
}
