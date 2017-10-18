namespace Trcont.Ris.Story.Contracts
{
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Dto;

    public class CreateContractBillByIdStoryContext
    {
        public int? ContractId { get; set; }

        public IEnumerable<AccountBillItemDto> AccountBillItems { get; set; }

        public Guid? UserGuid { get; set; }

        public bool IsClient { get; set; }
    }
}
