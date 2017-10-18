namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlRoot("MainRoot")]
    public class IrsXmlMainRoot
    {
        public IrsXmlMainRoot()
        {
            DeliveryRoute = new List<IrsXmlDeliveryRoute>();
        }

        public IrsXmlMainRoot(IrsXmlDeliveryRoute droute)
            : this()
        {
            DeliveryRoute.Add(droute);
        }

        //[XmlAttribute]
        //public int Id { get; set; }

        [XmlElement("DeliveryRoute")]
        public List<IrsXmlDeliveryRoute> DeliveryRoute { get; set; }
    }
}
