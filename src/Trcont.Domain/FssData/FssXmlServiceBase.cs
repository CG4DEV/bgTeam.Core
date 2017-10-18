namespace Trcont.Domain.FssData
{
    using System;
    using System.Xml.Serialization;
    using Trcont.Domain.Common;

    public abstract class FssXmlServiceBase
    {
        [XmlAttribute]
        public string USLCode { get; set; }

        [XmlAttribute]
        public decimal Cost { get; set; }

        [XmlAttribute]
        public int Currency { get; set; }

        [XmlAttribute]
        public int TariffType { get; set; }

        [XmlAttribute]
        public int StdVolume { get; set; }

        [XmlAttribute]
        public int Territory { get; set; }

        [XmlAttribute]
        public string RenderPlace { get; set; }

        [XmlAttribute]
        public string UseConditions { get; set; }

        [XmlAttribute]
        public string ContractRegNum { get; set; }

        [XmlAttribute]
        public string ActualPeriodStart { get; set; }

        [XmlAttribute]
        public string ActualPeriodEnd { get; set; }

        [XmlAttribute]
        public string ParentUSLCode { get; set; }

        [XmlAttribute("ServiceID")]
        public int ServiceId { get; set; }
    }
}
