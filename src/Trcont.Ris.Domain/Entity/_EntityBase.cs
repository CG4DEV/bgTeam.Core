namespace Trcont.Ris.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;

    public abstract class EntityBase
    {
        public int? Id { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? TimeStamp { get; set; }
    }

    public abstract class EntityBaseIdentity
    {
        [Identity]
        public int? Id { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? TimeStamp { get; set; }
    }

    public interface IIrsDictionary
    {
        Guid IrsGuid { get; set; }
    }
}
