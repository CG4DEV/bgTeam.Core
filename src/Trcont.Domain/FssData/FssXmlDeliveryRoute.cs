namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class FssXmlDeliveryRoute
    {
        public FssXmlDeliveryRoute()
        {
            DeliveryArmList = new List<FssXmlDeliveryArm>();
            RailPPListList = new List<FssXmlStation>();
        }

        [XmlAttribute]
        public string RouteId { get; set; }

        [XmlAttribute]
        public double Priority { get; set; }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public int Duration { get; set; }

        [XmlAttribute("SpecialRateUID")]
        public string SpecialRateUId { get; set; }

        [XmlAttribute]
        public string SpecTaxDescription { get; set; }

        [XmlAttribute]
        public string SpecTaxDocumentLink { get; set; }

        [XmlElement("DeliveryArm")]
        public List<FssXmlDeliveryArm> DeliveryArmList { get; set; }

        [XmlArray("RailPPList")]
        public List<FssXmlStation> RailPPListList { get; set; }

        //public DateTime ActualPeriodStart { get; set; }

        //public DateTime ActualPeriodEnd { get; set; }

        //public decimal Summ { get; set; }

        //public decimal SummVAT { get; set; }

        //public int CurrencyId { get; set; }

        //public string CurrencyTitle { get; set; }

        //public int Color { get; set; }
    }
}
