namespace Trcont.Ris.Domain.Entity
{
    using System;

    public class ServiceParam
    {
        public Guid ParamGuid { get; set; }

        public Guid? ParentGuid { get; set; }

        public int ParamType { get; set; }

        public int Code { get; set; }

        public string NameRus { get; set; }

        public string Description { get; set; }

        public Guid? DomainGuid { get; set; }

        public string DomainName { get; set; }

        public int IsEditAllow { get; set; }

        public int IsMultiValues { get; set; }

        public int IsVisible { get; set; }

        public bool IsEditUserValue { get; set; }
    }
}
