using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trcont.IRS.Domain.Dto
{
    public class ClientDogovorDto
    {
        public int Id { get; set; }

        public Guid ContractGuid { get; set; }

        public string RegNumber { get; set; }

        public string Name { get; set; }

        public DateTime ContractDate { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public bool? IsValid { get; set; }
    }
}
