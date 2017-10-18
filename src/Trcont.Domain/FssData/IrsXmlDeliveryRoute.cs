namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class IrsXmlDeliveryRoute
    {
        public IrsXmlDeliveryRoute()
        {
            DeliveryArm = new List<IrsXmlDeliveryArm>();
        }

        [XmlAttribute]
        public int RouteId { get; set; }

        [XmlAttribute]
        public int Priority { get; set; }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public int Duration { get; set; }

        [XmlElement("DeliveryArm")]
        public List<IrsXmlDeliveryArm> DeliveryArm { get; set; }
    }
}
