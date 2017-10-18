namespace Trcont.Cud.Domain.Entity.ExtInfo
{
    using System;

    public class DislocationStation
    {
        public Guid ReferenceGUID { get; set; }

        public string StationTitle { get; set; }

        public string StationCode { get; set; }

        public bool IsPort { get; set; }
    }
}
