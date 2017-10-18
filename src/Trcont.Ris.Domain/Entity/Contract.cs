namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class Contract : EntityBase
    {
        public Guid IrsGuid { get; set; }

        public int? CurrencyId { get; set; }

        public string RegNumber { get; set; }

        public int ClientId { get; set; }

        public string Comments { get; set; }

        public string Name { get; set; }

        public DateTime ContractDate { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int OurSideId { get; set; }

        public int ContractTypeId { get; set; }

        public int? ContractStateId { get; set; }

        public short CudStatus { get; set; }

        public string BIK { get; set; }

        public string BankTitle { get; set; }

        public string BankAddress { get; set; }

        public int? AccType { get; set; }
    }
}
