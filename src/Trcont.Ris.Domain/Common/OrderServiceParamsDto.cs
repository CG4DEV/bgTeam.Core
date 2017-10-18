namespace Trcont.Ris.Domain.Common
{
    using System;

    public class OrderServiceParamsDto
    {
        public int Id { get; set; }

        public int TeoId { get; set; }

        public int OrderId { get; set; }

        public int? OrderServiceId { get; set; }

        public Guid? OrderServiceGuid { get; set; }

        public int? TeoServiceId { get; set; }

        public Guid? TeoServiceGuid { get; set; }

        public Guid AttribGuid { get; set; }

        public string AttribName { get; set; }

        public string AttribValueRus { get; set; }

        public decimal? AttribNumValue { get; set; }

        public DateTime? AttribDateValue { get; set; }
    }
}
