namespace Trcont.Cud.Domain.Entity.ExtInfo
{
    using System;

    public class DislocationInfo
    {
        public string ContNumber { get; set; }

        public DateTime Date { get; set; }

        public Station Station { get; set; }

        public Value Operation { get; set; }
    }
}
