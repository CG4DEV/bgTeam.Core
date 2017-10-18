namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class ContractDto
    {
        public int Id { get; set; }

        public string RegNumber { get; set; }

        public string Name { get; set; }

        public Guid IrsGuid { get; set; }

        public DateTime ContractDate { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int StateId { get; set; }

        public string StateName { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public bool? IsValid { get; set; }

        public decimal Summ { get; set; }
    }
}
