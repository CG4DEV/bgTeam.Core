namespace Trcont.Domain.FssData
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("Root")]
    public class FssXmlRoot
    {
        public FssXmlRoot()
        {
            DeliveryRouteList = new List<FssXmlDeliveryRoute>();
        }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("DeliveryRoute")]
        public List<FssXmlDeliveryRoute> DeliveryRouteList { get; set; }
    }
}
