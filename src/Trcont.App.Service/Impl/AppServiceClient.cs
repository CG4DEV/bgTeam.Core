namespace Trcont.App.Service.Impl
{
    using System;
    using System.Linq;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.Extensions;
    using Trcont.App.Service.Entity;
    using Trcont.App.Service.TKApplicationService;
    using Trcont.Domain.Entity;

    public class AppServiceClient : IAppServiceClient
    {
        private readonly string _serviceAddress;

        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;

        public AppServiceClient(string serviceAddress, IAppLogger logger, IMapperBase mapper)
        {
            _serviceAddress = serviceAddress.CheckNull(nameof(serviceAddress));
            _logger = logger.CheckNull(nameof(logger));
            _mapper = mapper.CheckNull(nameof(mapper));
        }

        public async Task<string> SaveOrUpdateIrsOrderAsync(KpDocuments order)
        {
            var kpo = _mapper.Map(order, new KpDocumentObject());
            kpo.Services = Enumerable.Empty<KpServiceObject>().ToArray();

            kpo.CurrencyId = 1;

            var client = GetClient(_serviceAddress);


            //client.TeoIRSToLocal(new TeoIRSToLocalRequest() { teoGuid })

            var response = await client.KpSaveToIRSAsync(kpo, Guid.NewGuid());

            var code = response.ErrorCode;
            var message = response.ErrorMessage;

            return message;
        }

        public async Task<Guid?> SaveOrUpdateOrderAsync(OrderInfo order)
        {
            //var user = new Guid("6afde72a-1481-4f46-8aa7-084f88c8a07f"); // User portal

            var client = GetClient(_serviceAddress);

            var kpDoc = _mapper.Map(order, new KpDocumentObject());

            kpDoc.TeoGuid = null;
            //kpDoc.TemplateId = 207393; //Коммерческое предложение на оказание услуг ТЭО
            kpDoc.TemplateId = 58493501; //Коммерческое предложение на оказание услуг ТЭО в новой номенклатуре ЕПУ/ЕСУ
            //kpDoc.TemplateId = 207385;

            kpDoc.ExternalXML = order.ExternalXML;

            // Сохраняем кп в ИРСу
            var res = await client.KpSaveToIRSAsync(kpDoc, order.ManagerGuid);
            if (res.ErrorCode > 0)
            {
                throw new Exception(res.ErrorMessage);
            }

            // Для заказа приравниваем from и to
            // если заказ для определённого point
            order.Services.DoForEach(x =>
            {
                if (x.ServiceForPoint)
                {
                    x.PlaceToGuid = x.PlaceFromGuid;
                }
            });

            var teoDoc = _mapper.Map(order, new TeoDocumentObject());

            teoDoc.TeoGuid = Guid.NewGuid();
            teoDoc.TemplateId = 207385; //Заказ на оказание услуг ТЭО
            teoDoc.LoadToGuid = new Guid("5e03b366-e457-47d6-af5d-7cfb71057e6f");
            teoDoc.LoadFromGuid = new Guid("5e03b366-e457-47d6-af5d-7cfb71057e6f");

            // Преобразовываем в нужный тип
            teoDoc.Services = teoDoc.Services.Select(x => _mapper.Map(x, new TeoServiceObject())).ToArray();

            // Сохраняем заказ в ИРСу
            var res2 = await client.TeoSaveToIRSAsync(teoDoc, order.ManagerGuid); // User portal 
            if (res2.ErrorCode > 0)
            {
                throw new Exception(res2.ErrorMessage);
            }

            // Сохраняем заказ в КУД
            var res3 = await client.TeoIRSToLocalAsync(teoDoc.TeoGuid);
            if (res3.ErrorCode > 0)
            {
                throw new Exception(res3.ErrorMessage);
            }

            //return teoDoc.TeoGuid;
            return kpDoc.KpGuid;
        }

        private DocumentDto DocumentContentProcess(DocumentContentResponse response)
        {
            if (response.ErrorCode > 0)
            {
                return new DocumentDto() { IsError = true, ErrorMessage = response.ErrorMessage };
            }

            return new DocumentDto() { Content = response.Content, FileExtension = response.FileExtension };
        }

        public async Task<DocumentDto> GetBaseDocumentDocContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.BaseDocumentDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetBaseDocumentPdfContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.BaseDocumentPdfDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetContractDocumentDocContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.ContractContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetContractDocumentPdfContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.ContractPdfContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetKpDocumentDocContent(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.KpDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetKpDocumentPdfContent(Guid docGuid, Guid? landGuid)
        {
            //var client = GetClient(_serviceAddress);
            //var responce = await client.GetPDFKpContent(docGuid, landGuid);
            //return DocumentContentProcess(responce);

            throw new NotSupportedException("Нет реализации в интерфейсе ITKApplicationService");
        }

        public async Task<DocumentDto> GetTeoDocumentDocContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.TeoDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetTeoDocumentPdfContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.TeoPdfDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetBillTeoDocumentDocContentAsync(Guid docGuid, Guid? landGuid)
        {
            var client = GetClient(_serviceAddress);
            var responce = await client.TeoBillDocumentContentAsync(docGuid, landGuid);
            return DocumentContentProcess(responce);
        }

        public async Task<DocumentDto> GetBillTeoDocumentPdfContentAsync(Guid docGuid, Guid? landGuid)
        {
            //var client = GetClient(_serviceAddress);
            //var responce = await client.TeoBillDocumentContentAsync(docGuid, landGuid);
            //return DocumentContentProcess(responce);

            throw new NotSupportedException("Нет реализации в интерфейсе ITKApplicationService");
        }

        public async Task<Guid> CreateOrderBillAsync(Guid teoGuid, Guid currencyGuid, Guid userGuid)
        {
            var client = GetClient(_serviceAddress);
            var res = await client.SetTeoBillAsync(teoGuid, currencyGuid, userGuid);
            if (res.ErrorCode > 0)
            {
                throw new Exception(res.ErrorMessage);
            }

            return res.Guid;
        }

        public async Task<Guid> CreateContractAccountBillAsync(Guid contractGuid, int[] index, decimal[] billSumm, Guid userGuid, bool isClient)
        {
            var client = GetClient(_serviceAddress);

            var res = await client.CreateContractAccountBillAsync(contractGuid, index, billSumm, userGuid, isClient);
            if (res.ErrorCode > 0)
            {
                throw new Exception(res.ErrorMessage);
            }

            return res.BillGuid;
        }

        #region CreateClient
        private static TKApplicationServiceClient GetClient(string endPointAddress)
        {
            var client = new TKApplicationServiceClient(CreateDefaultBinding(), new EndpointAddress(endPointAddress));

            client.ChannelFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;

            return client;
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
        #endregion
    }
}
