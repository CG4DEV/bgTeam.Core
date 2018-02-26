namespace bgTeam.Extensions
{
    using System;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Расширения манипулирующие Xml-элементами
    /// </summary>
    public static class XmlExtension
    {
        /// <summary>
        /// Возвращает значение атрибута в XmlNode по его имени
        /// </summary>
        /// <param name="node">Нода</param>
        /// <param name="attributeName">Имя атрибута</param>
        /// <returns>Значение атрибута</returns>
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
