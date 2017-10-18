namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class IrsXmlDeliveryArm
    {
        public IrsXmlDeliveryArm()
        {
            ServiceMandatory = new List<IrsXmlServiceMandatory>();
        }

        [XmlAttribute]
        public int LevelUpServiceTypeID { get; set; }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public int FromPointId { get; set; }

        [XmlAttribute]
        public int ToPointId { get; set; }

        [XmlAttribute]
        public int Duration { get; set; }

        [XmlAttribute]
        public float Lenght { get; set; }

        [XmlElement("ServiceMandatory")]
        public List<IrsXmlServiceMandatory> ServiceMandatory { get; set; }
    }
}
