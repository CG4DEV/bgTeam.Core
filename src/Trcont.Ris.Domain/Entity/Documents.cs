namespace Trcont.Ris.Domain.Entity
{
    using System;
    using System.ComponentModel;

    public class Documents : EntityBase
    {
        public Guid IrsGuid { get; set; }

        public int DocumentsTypeId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public DateTime? DocumentDate { get; set; }
    }

    public enum DocumentsOwnerTypeEnum
    {
        None = 0,

        [Description("Заказ")]
        Orders = 1,

        [Description("Договор")]
        Contracts = 2
    }
}
