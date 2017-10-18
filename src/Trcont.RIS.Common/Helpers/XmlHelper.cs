namespace Trcont.Ris.Common.Helpers
{
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;

    public class XmlHelper
    {
        public static T DeserializeXmlFromFile<T>(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (FileStream fileStream = new FileStream(path, FileMode.Open))
            {
                var res = (T)serializer.Deserialize(fileStream);
                return res;
            }
        }

        public static T DeserializeXmlFromString<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(new StringReader(xmlString));
        }

        public static string SerializeToString<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            using (var sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw))
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }
    }
}
