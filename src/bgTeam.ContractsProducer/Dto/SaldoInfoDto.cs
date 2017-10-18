using System;
using System.Collections.Generic;

namespace bgTeam.ContractsProducer.Dto
{
    public class SaldoInfoDto
    {
        public Guid ContractGuid { get; set; }

        public string EntityType { get; set; }

        public string EntityKey { get; set; }

        public IEnumerable<SaldoOrderInfoDto> Orders { get; set; }

        public IEnumerable<SaldoSercviceInfoDto> Services { get; set; }
    }
}
