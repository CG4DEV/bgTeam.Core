namespace Trcont.Cud.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class OrderContainerNumbersDto
    {
        public Guid ReferenceGuid { get; set; }

        public IEnumerable<string> Numbers { get; set; }
    }
}
