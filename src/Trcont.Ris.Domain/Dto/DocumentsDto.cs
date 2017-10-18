namespace Trcont.Ris.Domain.Dto
{
    using DapperExtensions.Attributes;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Entity;

    public class DocumentsDto : Documents
    {
        public string DocumentsTypeName { get; set; }

        public int OwnerTypeId { get; set; }

        public int OwnerId { get; set; }

        public DateTime? DocumentDate { get; set; }

        public int? PrintDocType { get; set; }

        [Ignore]
        [JsonIgnore]
        public DocumentsOwnerTypeEnum OwnerType => (DocumentsOwnerTypeEnum)OwnerTypeId;
    }
}
