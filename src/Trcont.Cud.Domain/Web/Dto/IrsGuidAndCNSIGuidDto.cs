namespace Trcont.Cud.Domain.Web.Dto
{
    using System;

    public class IrsGuidAndCNSIGuidDto
    {
        public Guid CNSIGuid { get; set; }

        public Guid ReferenceGuid { get; set; }

        public string Title { get; set; }

        public string Account { get; set; }
    }
}
