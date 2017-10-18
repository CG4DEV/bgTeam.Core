namespace Trcont.Domain.FssData
{
    using System.Xml.Serialization;

    public class FssXmlStation
    {
        [XmlAttribute]
        public string Code { get; set; }
    }
}
