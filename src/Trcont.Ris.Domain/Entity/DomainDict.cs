namespace Trcont.Ris.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;

    [TableName("Domain")]
    public class DomainDict : EntityBaseIdentity
    {
        public Guid IrsGuid { get; set; }

        public string Title { get; set; }

        public string Descroption { get; set; }

        public int Type { get; set; }
    }
}
