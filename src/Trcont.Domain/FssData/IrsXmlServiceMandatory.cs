namespace Trcont.Domain.FssData
{
    using System;
    using System.Xml.Serialization;

    public class IrsXmlServiceMandatory : IrsXmlServiceBase
    {
        [XmlAttribute]
        public decimal Rate { get; set; }

        [XmlAttribute]
        public decimal ExchangeRate { get; set; }

        [XmlAttribute]
        public int TariffType { get; set; }

        [XmlAttribute]
        public int StdVolume { get; set; }

        [XmlAttribute]
        public string ContractId { get; set; }

        [XmlAttribute]
        public string ActualPeriodStart { get; set; }

        [XmlAttribute]
        public string ActualPeriodEnd { get; set; }

        [XmlAttribute]
        public string TerritoryId { get; set; }

        [XmlAttribute]
        public string Label { get; set; }
    }
}
