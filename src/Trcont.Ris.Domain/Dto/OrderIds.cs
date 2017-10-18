namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class OrderIds
    {
        public int Id { get; set; }

        public Guid IrsGuid { get; set; }

        public int TeoId { get; set; }

        public Guid TeoGuid { get; set; }

        public string Number { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
