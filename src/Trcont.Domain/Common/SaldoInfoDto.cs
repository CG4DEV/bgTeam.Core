namespace Trcont.Domain.Common
{
    using System;
    using System.Collections.Generic;

    public class SaldoInfoDto
    {
        public Guid ContractGuid { get; set; }

        public IEnumerable<SaldoOrderInfoDto> Orders { get; set; }

        public IEnumerable<SaldoServiceInfoDto> Services { get; set; }
    }
}
