namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.Web;
    using Trcont.App.Service;
    using Trcont.App.Service.Entity;
    using Trcont.App.Service.TKApplicationService;
    using Trcont.Ris.Common;
    using Trcont.Ris.DataAccess.Common;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Context;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Story.Orders.SaveOrUpdate;
    using Trcont.Ris.DataAccess.Order;
    using Dapper;
    using System.Text;

    public class SaveOrUpdateOrderStory : IStory<SaveOrUpdateOrderStoryContext, OrderIds>
    {
        private const string GET_ORDERID_BY_IRS_GUID = "Info/GetOrderIdByIrsGuid";

        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IAppServiceClient _appService;
        private readonly IOrderServiceService _orderSrvService;
        private readonly IConnectionFactory _connectionFactory;
        private readonly IFssToIrsConvertor _fssConvertor;
        private readonly IWebClient _webClient;

        private readonly Hashtable _cashList;

        public SaveOrUpdateOrderStory(
            IAppLogger logger,
            IMapperBase mapper,
            IRepository repository,
            IRepositoryEntity repositoryEn,
            IAppServiceClient appService,
            IOrderServiceService orderSrvService,
            IConnectionFactory connectionFactory,
            IFssToIrsConvertor fssConvertor,
            IWebClient webClient)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _repositoryEn = repositoryEn;
            _appService = appService;
            _orderSrvService = orderSrvService;
            _connectionFactory = connectionFactory;
            _fssConvertor = fssConvertor;
            _webClient = webClient;


            _cashList = new Hashtable();

            // Валюта
            _cashList.Add("RUB", 1);
            _cashList.Add("USD", 2);
            _cashList.Add("EUR", 71698);
        }

        public OrderIds Execute(SaveOrUpdateOrderStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrderIds> ExecuteAsync(SaveOrUpdateOrderStoryContext context)
        {
            if (context.CargoInfo == null && context.CargoInfo.Any())
            {
                throw new ArgumentNullException(nameof(context.CargoInfo));
            }

            var analizer = new RouteAnalizeService(_repositoryEn);
            await analizer.AnalizeAsync(context);

            var order = _mapper.Map(context, new OrderInfo());

            // определяем менеджера
            order.ManagerGuid = await LoadManager(context.ContractGuid);
            //order.ClientGuid = GetIrsGuidIfNotNull(irsGuids, context.ClientGuidCNSI, context.ClientGuid);

            order.ChosenRouteNum = 1; // все время 1. т.к мы сами формируем xml для ирсы
            order.CurrencyId = (_cashList[context.CurrencyCnsi] as int?) ?? 1;

            order.StationFromGuid = context.PlaceFromGuid;
            order.CountryFromGuid = (await new GetCountryByPointGuidCmd(_repository).ExecuteAsync(new GetCountryByPointGuidCmdContext() { PointGuid = order.StationFromGuid })).CountryGuid;
            order.StationToGuid = context.PlaceToGuid;
            order.CountryToGuid = (await new GetCountryByPointGuidCmd(_repository).ExecuteAsync(new GetCountryByPointGuidCmdContext() { PointGuid = order.StationToGuid })).CountryGuid;

            order.TrainTypeGuid = await GetRowByCodeGuid<ContainerType>(context.TrainTypeCnsi, x => x.CNSICode == context.TrainTypeCnsi);

            // опеределение груза
            if (context.CargoInfo.Count() == 1)
            {
                var cargo = context.CargoInfo.First();

                order.EtsngGuid = await GetRowByCodeGuid<Etsng>(cargo.ETSNGCode, x => x.Code == cargo.ETSNGCode.Substring(0, 5));
                order.GngGuid = await GetRowNullByCodeGuidAsync<GNG>(cargo.GNGCode, x => x.Code == cargo.GNGCode);
            }
            else
            {
                order.EtsngGuid = new Guid("dd09c22c-3ab1-41e1-a8ce-6ab06c49a203"); //99711 : СБОРНАЯ КОНТЕЙНЕРНАЯ ОТПРАВКА

                var sb = new StringBuilder();
                sb.Append("\r\n\r\nСБОРНЫЙ ГРУЗ:\r\n");
                foreach (var item in context.CargoInfo)
                {
                    sb.Append($"ЕТСНГ: {item.ETSNGCode}\r\n");
                    sb.Append($"ГНГ: {item.ETSNGCode}\r\n");
                    sb.Append($"\r\n");
                }

                if (context.TypeMessage != Domain.Enums.TypeMessageEnum.Внутрироссийская ||
                    context.TypeMessage != Domain.Enums.TypeMessageEnum.Внутригосударственная)
                {
                    order.GngGuid = new Guid("82ff2efc-57f0-48ec-89d1-08c703295a29"); //99020000 : ГРУЗЫ СБОРНЫЕ
                }

                order.Comment += sb.ToString();
            }

            // Стандартные параметры
            order.KpGuid = Guid.NewGuid();

            order.IsDefaultLanguage = true;
            order.CurrentLanguageGuid = new Guid("7250baf8-a095-4e0d-9787-6e3b91e411ef"); //Ru
            order.UserGuid = new Guid("e3d6af29-70d9-44c4-ac8b-85004f569520"); //00. ЦКП.администратор  Системный
            order.AccessDate = DateTime.Now;
            order.DocumentTitle = "Заказ из iSalesPro";
            //order.CustomsMode = 1;
            order.CustomsType = false;

            // Неизвестные параметры
            //order.RouteType = 0;
            order.RefSectionSize = null;
            order.WagonQuantity = 0;
            //order.ContrRelationsType = 1;
            order.RefreshExchRates = 0;

            // мапим значения
            order.OutCategory = int.Parse(context.OutCategory.GetDescription());
            order.Speed = int.Parse(context.Speed.GetDescription());
            order.SendingGuid = Guid.Parse(context.SendingType.GetDescription());

            var transTypeId = int.Parse(context.TypeMessage.GetDescription());
            var transType = await _repositoryEn.GetAsync<TransType>(x => x.Id == transTypeId);
            order.TransTypeId = transType.Id;
            order.TransTypeGuid = transType.IrsGuid;

            var index = 0;
            var list = new List<OrderServiceInfoExt>();
            foreach (var route in context.Routes)
            {
                foreach (var service in route.Services)
                {
                    var srv = await GetServiceByCodeAsync(service.ServiceCode);
                    if (srv == null)
                    {
                        throw new ArgumentNullException($"Not find service by code - {service.ServiceCode}");
                    }

                    if (!service.ServiceFortId.HasValue)
                    {
                        throw new ArgumentNullException($"Not find serviceFortId by code - {service.ServiceCode}");
                    }

                    var teo = new OrderServiceInfoExt()
                    {
                        ArmIndex = index,
                        //ActualStartDate = order.PeriodBeginDate,
                        //ActualEndDate = order.PeriodEndDate,

                        ServiceGuid = Guid.NewGuid(),

                        ServiceId = srv.Id.Value,
                        ServiceCode = srv.Code.Trim(),
                        ServiceTitle = srv.Title,
                        ServiceType = service.ServiceType,
                        ServiceFortId = service.ServiceFortId.Value,

                        // Это ServiceGuid, названо из за корявого сервиса
                        ServiceTypeGuid = srv.IrsGuid,

                        PlaceRenderGuid = route.PlaceRenderGuid,

                        TariffType = service.TariffType,
                        TariffGuid = await GetRowNullByCodeGuidAsync<Unit>(service.TariffType.ToString(), x => x.Code == service.TariffType),
                        Tariff = service.Tariff,
                        TariffVAT = service.TariffVAT,

                        SrcVolume = service.SrcVolume,
                        //Summ = service.Tariff * order.ContainerQuantity,
                        //SummVAT = (service.Tariff / 100) * (service.TariffVAT),
                        Summ = service.Summ,
                        SummVAT = service.SummVAT,

                        IsProfit = 1,
                        DangerClass = 0,
                        DangerSubClass = 0,
                        SrvGroupType = 7,

                        //Duration = 0,
                        ConvertRate = 1.0000m,
                        SourceCurrencyId = order.CurrencyId,

                        ExtRateSource = 0,
                        ParentType = -1,

                        PayAccountId = route.PayAccountId,
                        TerritoryGuid = service.TerritoryGuid,
                        //ExecGuid = null,
                        //CustomsType = false,

                        SourcePriceServiceId = service.SourcePriceServiceId,
                        SourceReferenceId = service.SourceReferenceId,

                        PlaceFromGuid = route.FromPointGuid.Value,
                        FromPointCode = route.FromPointCode,
                        FromPointId = route.FromPointId,

                        PlaceToGuid = route.ToPointGuid.Value,
                        ToPointCode = route.ToPointCode,
                        ToPointId = route.ToPointId,

                        IsActive = true,
                        IsRequired = (int)service.ServiceType,
                    };

                    teo.Route = route;
                    list.Add(teo);
                }

                index++;
            }

            await _orderSrvService.FillAttributesAsync(order, list);

            // мапим атрибуты услуг
            var srvMass = list.DoForEach(x => x.Attributes = x.Attributes2.Select(z => _mapper.Map(z, new LtrAttributeObject())).ToArray()).ToArray();
            order.ExternalXML = await _fssConvertor.ConvertAsync(order.ExternalXML, context.RouteId, context.RouteSpecId, srvMass);
            order.Services = srvMass;

            var res = await _appService.SaveOrUpdateOrderAsync(order);
            if (!res.HasValue)
            {
                throw new Exception("SaveOrUpdateOrderStory: could not save order");
            }

            var webContext = new GetOrderIdByIrsGuidStoryContext() { ReferenceGuid = res.Value };
            var info = await _webClient.PostAsync<OrderInfoByGuid>(GET_ORDERID_BY_IRS_GUID, webContext);

            var orderIds = _mapper.Map(info, new OrderIds());

            var saveContext = new SaveOrderLocalCmdContext()
            {
                Order = order,
                OrderInfo = info,
                OrderRoutes = context.Routes,
                OrderCargo = context.CargoInfo,
            };

            // Сохранить заказ в БД Ris
            await new SaveOrderLocalCmd(_connectionFactory, _repositoryEn, _repository, _mapper, _cashList).ExecuteAsync(saveContext);

            return orderIds;
        }

        private async Task<Guid> LoadManager(Guid? contractGuid)
        {
            if (!contractGuid.HasValue)
            {
                return Guid.Empty;
            }

            var sql = @"
                SELECT 
                  ISNULL(p.IrsGuid, cr.ExternalUser) AS UserGuid
                FROM Contract c WITH(NOLOCK)
                  INNER JOIN ContractResponsibles cr WITH(NOLOCK) ON c.Id = cr.ContractId
                  LEFT JOIN People p WITH(NOLOCK) ON p.Id = cr.UserId
                WHERE c.IrsGuid = @ContractGuid AND
                  (cr.BeginDate IS NULL OR cr.BeginDate IS NOT NULL AND cr.BeginDate <= @CurrentDate) AND
                  (cr.EndDate IS NULL OR cr.EndDate IS NOT NULL AND cr.EndDate >= @CurrentDate)
                ORDER BY cr.BeginDate DESC";

            var userGuids = await _repository.GetAllAsync<Guid>(sql, new { ContractGuid = contractGuid.Value, CurrentDate = DateTime.Now.Date });
            
            //TODO : целесобразность данной проверки
            //if (userGuids.Count() > 1)
            //{
            //    throw new Exception("У договора несколько ответственных");
            //}

            return userGuids.FirstOrDefault();
        }

        #region Получение справочных значение
        private async Task<Guid?> GetRowNullByCodeGuidAsync<T>(string code, Expression<Func<T, bool>> predicate)
            where T : class, IIrsDictionary
        {
            if (code == null)
            {
                return null;
            }

            if (_cashList.ContainsKey(code))
            {
                return _cashList[code] as Guid?;
            }

            var points = await _repositoryEn.GetAllAsync<T>(predicate);
            if (points.Any())
            {
                var point = points.First();
                _cashList.Add(code, point.IrsGuid);
                _cashList.Add($"{code}_ORIG", point);
                return point.IrsGuid;
            }

            //if (defValue != null)
            //{
            //    _cashList.Add(code, defValue);
            //}

            return null;
        }

        private async Task<Guid> GetRowByCodeGuid<T>(string code, Expression<Func<T, bool>> predicate)
            where T : class, IIrsDictionary
        {
            var res = await GetRowNullByCodeGuidAsync(code, predicate);

            if (!res.HasValue)
            {
                throw new ArgumentNullException($"Not find {typeof(T).Name}, code - {code}");
            }

            return res.Value;
        }

        private async Task<Service> GetServiceByCodeAsync(string serviceCode)
        {
            if (_cashList.ContainsKey(serviceCode))
            {
                return _cashList[serviceCode] as Service;
            }

            var srvs = await _repositoryEn.GetAllAsync<Service>(x => x.Code == serviceCode && x.ServiceGroupId == (int)ServiceGroupEnum.EPU);
            if (srvs.Any())
            {
                var srv = srvs.First();
                _cashList.Add(serviceCode, srv);
                return srv;
            }

            return null;
        }
        #endregion
    }
}