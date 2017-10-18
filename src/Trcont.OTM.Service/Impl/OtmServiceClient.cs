namespace Trcont.OTM.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using bgTeam;
    using Trcont.Domain.OtmData;
    using Trcont.OTM.Service.WebService;

    public class OtmServiceClient : IOtmServiceClient
    {
        private readonly string _endpointAddress;
        private readonly string _login;
        private readonly string _password;
        private readonly IAppLogger _logger;

        public OtmServiceClient(string otmServiceAddress, string login, string password, IAppLogger logger)
        {
            _endpointAddress = otmServiceAddress;
            _login = login;
            _password = password;
            _logger = logger;
        }

        public async Task<string> SendTransmission(OtmRelease otmRelease)
        {
            IntXmlClient client = GetClient();

            Transmission transmission = new Transmission();
            Release release = ReleaseHelper.GetRelease(otmRelease);

            transmission.TransmissionHeader = new TransmissionHeader()
            {
                UserName = _login,
                Password = new PasswordType() { Value = _password }
            };
            transmission.TransmissionBody = new[]
            {
                new GLogXMLElement()
                {
                    Item = release
                }
            };
            
            var xml = SerializeToString(transmission);

            // TODO: HACK
            xml = xml.Replace("xsi:type=\"Release\"", string.Empty);

            var response = await RequestInternal(xml);

            if (string.IsNullOrEmpty(response.StackTrace))
            {
                return response.EchoedTransmissionHeader?.TransmissionHeader?.ReferenceTransmissionNo;
            }
            else
            {
                throw new OtmException($"Ошибка при обращении к серверу ОТМ: {_endpointAddress}", response);
            }
        }

        private async Task<TransmissionAck> RequestInternal(string xml)
        {
            WebRequest request = WebRequest.Create(_endpointAddress);
            byte[] xmlBytes = Encoding.UTF8.GetBytes(xml);

            request.Method = "POST";
            request.ContentLength = xmlBytes.Length;
            request.ContentType = "text/xml;charset=utf-8";

            using (var stream = request.GetRequestStream())
            {
                stream.Write(xmlBytes, 0, xmlBytes.Length);
            }

            WebResponse response = await request.GetResponseAsync();

            byte[] bytes = new byte[0];
            using (var stream = response.GetResponseStream())
            {
                var buffer = new byte[1024];
                var readBytes = 0;

                do
                {
                    readBytes = stream.Read(buffer, 0, buffer.Length);
                    bytes = bytes.Concat(buffer.Take(readBytes).ToArray()).ToArray();
                }
                while (readBytes > 0);
            }

            string respStr = Encoding.UTF8.GetString(bytes);

            return DeserializeXmlFromString<TransmissionAck>(respStr);
        }

        public static T DeserializeXmlFromString<T>(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            return (T)serializer.Deserialize(new StringReader(xmlString));
        }

        private static string SerializeToString<T>(T obj)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new[] { typeof(Release) });

            using (var sw = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sw, new XmlWriterSettings() { Encoding = Encoding.UTF8 }))
            {
                serializer.Serialize(writer, obj);
                return sw.ToString();
            }
        }

        private IntXmlClient GetClient()
        {
            return new IntXmlClient(CreateDefaultBinding(), new EndpointAddress(_endpointAddress));
        }

        private static Binding CreateDefaultBinding()
        {
            return new BasicHttpBinding
            {
                MaxReceivedMessageSize = 2147483647L,
                MaxBufferPoolSize = 2147483647L,
                OpenTimeout = new TimeSpan(0, 30, 0),
                CloseTimeout = new TimeSpan(0, 30, 0),
                ReceiveTimeout = new TimeSpan(0, 30, 0),
                SendTimeout = new TimeSpan(0, 30, 0),
                ReaderQuotas =
                    {
                        MaxArrayLength = 2147483647,
                        MaxBytesPerRead = 2147483647,
                        MaxDepth = 2147483647,
                        MaxNameTableCharCount = 2147483647,
                        MaxStringContentLength = 2147483647
                    }
            };
        }
    }
}
