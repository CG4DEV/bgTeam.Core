namespace Trcont.IRS.Domain.Dto
{
    using System;

    public class FirmInfoDto
    {
        public Guid ReferenceGUID { get; set; }

        public string FullName { get; set; }

        public string Name { get; set; }

        public string RegisterType { get; set; }

        public string INN  { get; set; }

        public string OKPO { get; set; }

        public string KPP { get; set; }

        public string OGRN { get; set; }

        public string Director { get; set; }
    }
}
