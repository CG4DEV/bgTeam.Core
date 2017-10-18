namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class AutoZoneDto
    {
        public TypeValue City { get; set; }

        public CodeValue Region { get; set; }

        public TypeValue Street { get; set; }

        public TypeValue Town { get; set; }

        public string CnsiCode { get; set; }

        public class CodeValue
        {
            public Guid Code { get; set; }

            public string Name { get; set; }
        }

        public class TypeValue
        {
            public int? Type { get; set; }

            public string Title { get; set; }
        }
    }
}
