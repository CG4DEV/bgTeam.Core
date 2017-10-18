namespace Trcont.Fss.Service.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using bgTeam.Extensions;
    using Trcont.Fss.Service.Entity;

    public static class XmlResponseConvertor
    {
        public static CalcTariffResponse ConvertCalcTariffResponse(string xmlResponse)
        {
            xmlResponse.CheckNull(nameof(xmlResponse));
            XmlDocument document = new XmlDocument();
            document.LoadXml(xmlResponse);

            if (!document.HasChildNodes)
            {
                ThrowCommonException();
            }

            var result = new CalcTariffResponse();

            foreach (XmlNode xmlNode in document.ChildNodes)
            {
                ReadAttributes(xmlNode, result);
                ReadChildNodes(xmlNode, result);
            }

            return result;
        }

        private static void ReadAttributes(XmlNode node, object entity)
        {
            foreach (XmlAttribute attribute in node.Attributes)
            {
                entity.SetProperty(attribute.Name, attribute.Value);
            }
        }

        private static void ReadChildNodes(XmlNode node, object entity)
        {
            foreach (XmlNode xmlNode in node.ChildNodes)
            {
                var propertyInstance = CreateInstance(xmlNode.Name, entity);

                ReadAttributes(xmlNode, propertyInstance);
                ReadChildNodes(xmlNode, propertyInstance);

                entity.SetProperty(xmlNode.Name, propertyInstance);
            }
        }

        private static object CreateInstance(string name, object entity)
        {
            var type = entity.GetPropertyType(name);
            if (type != null)
            {
                return Activator.CreateInstance(type);
            }

            type = entity.GetPropertyType(name + "List");
            if (type == null)
            {
                throw new Exception("Неизвестное поле");
            }

            return Activator.CreateInstance(type.GetGenericArguments()[0]);
        }

        private static void ThrowCommonException()
        {
            throw new Exception("Ошибка парсинга ответа из ФСС");
        }
    }
}
