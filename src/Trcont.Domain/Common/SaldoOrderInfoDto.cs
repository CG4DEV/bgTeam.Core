namespace Trcont.Domain.Common
{
    using System;

    public class SaldoOrderInfoDto
    {
        public int? Id { get; set; }

        public int ServiceId { get; set; }

        public int AccountId { get; set; }

        public Guid RequestGuid { get; set; }

        public string RequestTitle { get; set; }

        public decimal BlockSum { get; set; }

        public decimal MoveSum { get; set; }

        public decimal ResultBlockSum { get; set; }
    }
}
