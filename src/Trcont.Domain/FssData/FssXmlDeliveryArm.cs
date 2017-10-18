namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class FssXmlDeliveryArm
    {
        public FssXmlDeliveryArm()
        {
            ServiceMandatoryList = new List<FssXmlServiceMandatory>();
            ServiceRecommendedList = new List<FssXmlServiceRecommended>();
            ExtraServiceList = new List<FssXmlExtraService>();
        }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public int Duration { get; set; }

        [XmlAttribute]
        public float Lenght { get; set; }

        [XmlAttribute]
        public string FromPointCode { get; set; }

        [XmlAttribute]
        public string ToPointCode { get; set; }

        [XmlElement("ServiceMandatory")]
        public List<FssXmlServiceMandatory> ServiceMandatoryList { get; set; }

        [XmlElement("ServiceRecommended")]
        public List<FssXmlServiceRecommended> ServiceRecommendedList { get; set; }

        [XmlElement("ExtraService")]
        public List<FssXmlExtraService> ExtraServiceList { get; set; }

        //public int FromPointId { get; set; }

        //public Guid FromPointGUID { get; set; }

        //public int ToPointId { get; set; }

        //public Guid ToPointGUID { get; set; }

        public List<FssXmlServiceBase> GetAllService()
        {
            var list = new List<FssXmlServiceBase>();

            list.AddRange(ServiceMandatoryList);
            list.AddRange(ServiceRecommendedList);
            list.AddRange(ExtraServiceList);

            return list;
        }
    }
}
