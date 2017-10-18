namespace Trcont.CitTrans.Service.Impl
{
    using bgTeam;
    using bgTeam.Extensions;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Trcont.CitTrans.Service.CitTrans;
    using Trcont.CitTrans.Service.Entity;
    using System.ServiceModel.Channels;
    using System;
    using System.ServiceModel;

    public class CitTransServiceClient : ICitTransServiceClient
    {
        private readonly IAppLogger _logger;
        private readonly string _serviceAddress;
        private readonly IMapperBase _mapper;

        public CitTransServiceClient(IAppLogger logger, string serviceAddress, IMapperBase mapper)
        {
            _logger = logger.CheckNull(nameof(logger));
            _serviceAddress = serviceAddress.CheckNull(nameof(serviceAddress));
            _mapper = mapper.CheckNull(nameof(mapper));
        }

        public async Task<IEnumerable<DislocationInfo>> GetOrderDislokAsync(string orderNumber, string containerNumber)
        {
            var client = GetClient();

            if (!string.IsNullOrWhiteSpace(orderNumber))
            {
                var request = new Get_Container_Dislocation()
                {
                    IsTestMessage = false,
                    Initiator = new Values() { Code = 1, Name = "ir000a - TransContainer" },
                    KontNumber = containerNumber ?? String.Empty,
                    OrderNumber = orderNumber
                };

                var response = await client.GetContainerDislocationAsync(request);

                if (response.ContainerDislocation == null ||
                    response.ContainerDislocation.ContainetDislocationInfo == null)
                {
                    return Enumerable.Empty<DislocationInfo>();
                }

                return response.ContainerDislocation.ContainetDislocationInfo
                        .Select(x => _mapper.Map(x, new DislocationInfo()))
                        .ToArray();
            }
            else
            {
                var response = await client.Inquiry_Container_StateAsync(new Container_State_Inquiry()
                {
                    ContainerNumber = containerNumber,
                    Initiator = new Values()
                    {
                        Code = 1,
                        Name = "ir000a - TransContainer"
                    },
                    IsTestMessage = false
                });

                var container = response.Container_State_Info.Container;
                if (container == null ||
                    string.IsNullOrWhiteSpace(container.ContainerNumber) ||
                    container.ContainerNumber == "0")
                {
                    return Enumerable.Empty<DislocationInfo>();
                }

                return new[] { new DislocationInfo(container) };
            }
        }

        private WebService_CitTrans_BizTalkSoapClient GetClient()
        {
            return new WebService_CitTrans_BizTalkSoapClient(CreateDefaultBinding(), new EndpointAddress(_serviceAddress));
        }

        private static Binding CreateDefaultBinding()
        {
            return new System.ServiceModel.BasicHttpBinding
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
