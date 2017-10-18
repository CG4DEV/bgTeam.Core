namespace Trcont.Cud.Domain.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Templates
    {
        public Guid TemplateGuid { get; set; }

        public string TemplateName { get; set; }

        //public int TemplateName_GUID { get; set; }

        public int TemplateId { get; set; }

        public int TemplateTypeId { get; set; }

        public int TemplateActive { get; set; }

        public int TemplateDogRF { get; set; }

        public string TemplateSystemName { get; set; }

        public int TemplatePrincipal { get; set; }
    }
}
