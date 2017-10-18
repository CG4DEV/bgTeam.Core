namespace bgTeam.ContractsQueryFactory.Dto
{
    using System;
    using System.Collections.Generic;

    public class SaldoInfoDto
    {
        public Guid ContractGuid { get; set; }

        public string EntityType { get; set; }

        public string EntityKey { get; set; }

        public IEnumerable<SaldoOrderInfoDto> Orders { get; set; }

        public IEnumerable<SaldoServiceInfoDto> Services { get; set; }
    }
}
