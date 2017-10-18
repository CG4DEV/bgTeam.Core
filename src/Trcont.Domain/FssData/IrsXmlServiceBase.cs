namespace Trcont.Domain.FssData
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public abstract class IrsXmlServiceBase
    {
        [XmlAttribute]
        public int ServiceTypeId { get; set; }

        [XmlAttribute]
        public int CurrencyId { get; set; }

        [XmlAttribute]
        public string UseConditions { get; set; }
    }
}
