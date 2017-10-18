namespace Trcont.Ris.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;

    [TableName("DomainsValues")]
    public class DomainDictValues : EntityBaseIdentity
    {
        public Guid IrsGuid { get; set; }

        public Guid DomainGuid { get; set; }

        public string Value { get; set; }
    }
}
