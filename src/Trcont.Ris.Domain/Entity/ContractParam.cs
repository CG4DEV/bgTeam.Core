namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class ContractParam
    {
        public long Id { get; set; }

        public int ParamId { get; set; }

        public int ContractId { get; set; }

        public string ParamName { get; set; }

        public string ParamTitle { get; set; }

        public string DefValue1 { get; set; }

        public int? ParamInt1 { get; set; }

        public string ParamStr1 { get; set; }

        public DateTime? ParamDateTime1 { get; set; }

        public float? ParamFloat1 { get; set; }

        public string DefValue2 { get; set; }

        public int? ParamInt2 { get; set; }

        public bool? IsVisible { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
