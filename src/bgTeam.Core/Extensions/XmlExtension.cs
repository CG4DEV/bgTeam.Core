namespace bgTeam.Extensions
{
    using System;
    using System.Linq;
    using System.Xml;

    public static class XmlExtension
    {
        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            if (node.Attributes.Count == 0)
            {
                throw new ArgumentException($"Не найден аттрибут с именем {attributeName}");
            }

            var attribute = node.Attributes
                .Cast<XmlAttribute>()
                .SingleOrDefault(x => x.Name == attributeName);

            if (attribute == null)
            {
                throw new ArgumentException($"Не найден аттрибут с именем {attributeName}");
            }

            return attribute.Value;
        }
    }
}
