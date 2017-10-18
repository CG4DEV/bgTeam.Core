namespace Trcont.Fss.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.Extensions;
    using Newtonsoft.Json;
    using Trcont.Fss.Service.Entity;
    using Trcont.Fss.Service.MMTCService;
    using Trcont.Fss.Service.Tools;

    public class FssServiceClient : IFssServiceClient
    {
        private readonly IAppLogger _logger;

        private readonly string _serviceAddress;
        private readonly string _login;
        private readonly string _password;

        public FssServiceClient(IAppLogger logger, string serviceAddress, string login = null, string password = null)
        {
            _logger = logger.CheckNull(nameof(logger));

            _serviceAddress = serviceAddress.CheckNull(nameof(serviceAddress));

            _login = login;
            _password = password;
        }

        private static MMTCService.MMTCCalculateTariffParams BuildCalcTariffRequestParams(CalcTariffRequestParams param)
        {
            return new MMTCService.MMTCCalculateTariffParams()
            {
                ContOwner = param.ContOwner,
                ContPark = param.ContPark,
                ContTypeCode = param.ContTypeCode,
                Date = param.Date,
                ETSNGcode = param.ETSNGcode,
                FromPointCode = param.FromPointCode,
                GNGCode = param.GNGCode,
                IsСontTrain = param.IsСontTrain,
                NettoWeight = param.NettoWeight,
                Rank = param.Rank,
                Speed = param.Speed,
                ToPointCode = param.ToPointCode,
                TotalWeight = param.TotalWeight,
                WagonOwner = param.WagonOwner,
                WagonPark = param.WagonPark,
                WagonTypeCode = param.WagonTypeCode,
                DeclaredCost = param.DeclaredCost
            };
        }

        public async Task<CalcTariffResponse> DoCalculateTariffAsync (CalcTariffRequestParams param)
        {
            param.CheckNull(nameof(param));

            var requestParam = BuildCalcTariffRequestParams(param);
            var client = GetClient(_serviceAddress, _login, _password);

            var responce = await client.DoCalculateTariffAsync(new MMTCService.DoCalculateTariffRequest(requestParam));
            return ProcessCalcTariffRespose(param, responce.DoCalculateTariffResult);
        }

        public CalcTariffResponse DoCalculateTariff(CalcTariffRequestParams param)
        {
            param.CheckNull(nameof(param));

            var requestParam = BuildCalcTariffRequestParams(param);
            var client = GetClient(_serviceAddress, _login, _password);

            var responce = client.DoCalculateTariff(new MMTCService.DoCalculateTariffRequest(requestParam));
            return ProcessCalcTariffRespose(param, responce.DoCalculateTariffResult);
        }

        private CalcTariffResponse ProcessCalcTariffRespose(CalcTariffRequestParams param, MMTCCalculateTariffResponse result)
        {
            if (result.HasErrors)
            {
                _logger.Error($"Request: {JsonConvert.SerializeObject(param)} - Response: {result.ResponseXml}");
                throw new Exception($"Получен ответ с ошибкой: {result.ResponseXml}");
            }

            _logger.Debug($"Request: {JsonConvert.SerializeObject(param)} - Response: {result.ResponseXml}");
            return XmlResponseConvertor.ConvertCalcTariffResponse(result.ResponseXml);
        }

        #region CreateClient
        private static MMTCService.MMTCServiceClient GetClient(string endPointAddress, string login, string password)
        {
            var client = new MMTCService.MMTCServiceClient(CreateDefaultBinding(), new EndpointAddress(endPointAddress));

            if (!string.IsNullOrEmpty(login))
            {
                client.ChannelFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Delegation;

                client.ClientCredentials.Windows.ClientCredential.Domain = "TK";
                client.ClientCredentials.Windows.ClientCredential.UserName = login;
                client.ClientCredentials.Windows.ClientCredential.Password = password;
            }
            else
            {
                client.ChannelFactory.Credentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
            }

            return client;
        }

        private static Binding CreateDefaultBinding()
        {
            return new WSHttpBinding
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
