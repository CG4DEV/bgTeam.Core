namespace Trcont.Cud.Story.Orders
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.Web;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;
    using Trcont.App.Service;
    using Trcont.App.Service.Entity;
    using Trcont.App.Service.TKApplicationService;
    using Trcont.Cud.DataAccess.Dictionaries;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Entity;
    using Trcont.Cud.Domain.Enum;
    using Trcont.Cud.Domain.Web.Context;
    using Trcont.Cud.Domain.Web.Dto;
    using Trcont.Domain;
    using Trcont.Domain.Entity;

    public class SaveOrUpdateOrderStory : IStory<SaveOrUpdateOrderStoryContext, Guid>
    {
        private readonly IAppLogger _logger;
        private readonly IMapperBase _mapper;
        private readonly IRepository _repository;
        private readonly IRepositoryEntity _repositoryEn;
        private readonly IAppServiceClient _appService;
        private readonly IWebClient _webClient;

        private const string GET_IRS_GUID_BY_CNSI_GUID = "api/Info/GetIrsGuidByCNSIGuid";

        public SaveOrUpdateOrderStory(
            IAppLogger logger,
            IMapperBase mapper,
            IRepository repository,
            IRepositoryEntity repositoryEn,
            IAppServiceClient appService,
            IWebClient webClient)
        {
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
            _repositoryEn = repositoryEn;
            _appService = appService;
            _webClient = webClient;
        }

        public Guid Execute(SaveOrUpdateOrderStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<Guid> ExecuteAsync(SaveOrUpdateOrderStoryContext context)
        {
            var groups = new List<GuidsGroup>();
            var irsGuids = await MapGuids(groups, context);
            var order = _mapper.Map(context, new OrderInfo());

            order.ClientGuid = GetIrsGuidIfNotNull(irsGuids, context.ClientGuidCNSI, context.ClientGuid);
            order.StationFromGuid = GetIrsGuidIfNotNull(irsGuids, context.PlaceFromGuidCNSI, context.PlaceFromGuid);
            order.StationToGuid = GetIrsGuidIfNotNull(irsGuids, context.PlaceToGuidCNSI, context.PlaceToGuid);
            order.TrainTypeGuid = GetIrsGuidIfNotNull(irsGuids, context.TrainTypeGuidCNSI, context.TrainTypeGuid);

            //order.EtsngGuid = GetIrsGuid(irsGuids, context.ETSNGGuid);
            //if (context.GNGGuid.HasValue)
            //{
            //    order.GngGuid = GetIrsGuid(irsGuids, context.GNGGuid.Value);
            //}

            // Стандартные параметры
            order.KpGuid = Guid.NewGuid();
            order.CurrencyId = GetCurrencyIdByGuid(context.CurrencyGuid);

            order.ChosenRouteNum = context.RouteId;
            order.TransTypeId = 27060; //Внутриросийская
            order.CurrentLanguageGuid = new Guid("7250baf8-a095-4e0d-9787-6e3b91e411ef"); //Ru
            order.UserGuid = new Guid("e3d6af29-70d9-44c4-ac8b-85004f569520"); //00. ЦКП.администратор  Системный
            order.AccessDate = DateTime.Now;
            order.DocumentTitle = "Заказ из iSalesPro";
            //order.CustomsMode = 1;
            order.IsDefaultLanguage = true;

            // Неизвестные параметры
            //order.RouteType = 0;
            order.RefSectionSize = null;
            order.WagonQuantity = 0;
            order.ContrRelationsType = 1;
            order.RefreshExchRates = 0;

            var index = 0;
            var list = new List<OrderServiceInfoExt>();
            foreach (var route in context.Routes)
            {
                foreach (var service in route.Services)
                {
                    var teo = new OrderServiceInfoExt()
                    {
                        ArmIndex = index,
                        //ActualStartDate = order.PeriodBeginDate,
                        //ActualEndDate = order.PeriodEndDate,

                        ServiceGuid = Guid.NewGuid(),
                        ServiceTypeGuid = service.ServiceGuid,
                        PlaceRenderGuid = route.PlaceRenderGuid,

                        TariffType = service.TariffType,
                        Tariff = service.Tariff,
                        TariffVAT = service.TariffVAT,

                        SrcVolume = order.ContainerQuantity,
                        Summ = service.Tariff * order.ContainerQuantity,
                        SummVAT = (service.Tariff / 100) * (service.TariffVAT),

                        IsProfit = 1,
                        DangerClass = 0,
                        DangerSubClass = 0,
                        SrvGroupType = 7,

                        //Duration = 0,
                        ConvertRate = 1.0000m,
                        SourceCurrencyId = GetCurrencyIdByGuid(context.CurrencyGuid),

                        ExtRateSource = 0,
                        ParentType = -1,

                        PayAccountId = route.PayAccountId,
                        TerritoryGuid = service.TerritoryGuid,
                        //ExecGuid = null,
                        //CustomsType = false,

                        SourcePriceServiceId = service.SourcePriceServiceId,
                        SourceReferenceId = service.SourceReferenceId,

                        PlaceFromGuid = GetIrsGuidIfNotNull(irsGuids, route.FromPointGuidCNSI, route.FromPointGuid).Value,
                        PlaceToGuid = GetIrsGuidIfNotNull(irsGuids, route.ToPointGuidCNSI, route.ToPointGuid).Value,

                        ServiceForPoint = service.ServiceForPoint,
                        //IsActive = true,
                        //IsRequired = 1,
                    };

                    teo.Route = route;
                    list.Add(teo);
                }

                route.FromPointGuid = GetIrsGuidIfNotNull(irsGuids, route.FromPointGuidCNSI, route.FromPointGuid).Value;
                route.ToPointGuid = GetIrsGuidIfNotNull(irsGuids, route.ToPointGuidCNSI, route.ToPointGuid).Value;

                //route.Etsng = GetIrsGuidIfNotNull(irsGuids, route.Etsng);
                route.SenderGuid = GetIrsGuidIfNotNull(irsGuids, route.SenderGuidCNSI, route.SenderGuid);
                route.ReceiverGuid = GetIrsGuidIfNotNull(irsGuids, route.ReceiverGuidCNSI, route.ReceiverGuid);
                route.AgentPortFrom = GetIrsGuidIfNotNull(irsGuids, route.AgentPortFromCNSI, route.AgentPortFrom);
                route.AgentPortTo = GetIrsGuidIfNotNull(irsGuids, route.AgentPortToCNSI, route.AgentPortTo);

                index++;
            }

            // Навешиваем атрибуты на услуги
            var massGuid = list
                .Select(x => x.ServiceTypeGuid)
                .ToArray();

            var serviceMap = await new GetParamsIdForServicesCmd(_repository)
                .ExecuteAsync(new GetParamsIdForServicesCmdContext() { ServiceGuids = massGuid });

            var paramList = new RouteServiceParamList();

            foreach (var item in list)
            {
                var list2 = serviceMap.Where(x => x.ServiceGuid == item.ServiceTypeGuid);

                item.Attributes = list2
                    .SelectMany(x => GetAttribute(paramList, x.ParameterTypeId, item.Route, order))
                    .Where(x => x != null)
                    .ToArray();
            }

            order.Services = list.ToArray(); //list.Select(x => _mapper.Map(x, new TeoServiceObject())).ToArray();

            var res = await _appService.SaveOrUpdateOrderAsync(order);

            if (!res.HasValue)
            {
                throw new Exception("SaveOrUpdateOrderStory: could not save order");
            }

            return res.Value;
        }

        private static Guid? GetIrsGuidIfNotNull(IEnumerable<IrsGuidAndCNSIGuidDto> list, Guid? input, Guid? defValue = null)
        {
            if (input.HasValue)
            {
                return GetIrsGuid(list, input.Value);
            }

            return defValue;
        }

        private static Guid GetIrsGuid(IEnumerable<IrsGuidAndCNSIGuidDto> list, Guid cnsiGuid)
        {
            var elem = list.SingleOrDefault(x => x.CNSIGuid == cnsiGuid);
            if (elem == null)
            {
                throw new Exception($"ИРС идентификатор {cnsiGuid} не найден. Обратитесь в тех.поддержку");
            }

            return elem.ReferenceGuid;
        }

        private IEnumerable<LtrAttributeObject> GetAttribute(RouteServiceParamList paramList, int parameterTypeId, RouteForSave route, OrderInfo order)
        {
            if (paramList.RouteServiceParam.ContainsKey(parameterTypeId))
            {
                var attrParam = paramList.RouteServiceParam[parameterTypeId];

                var value = attrParam.Property.GetValue(route, null);
                LtrAttributeObject attr = GetAttribute(attrParam.Id, attrParam.AttGuid, route.PlaceRenderGuid, value);

                return new[] { attr };
            }
            else
            {
                switch (parameterTypeId)
                {
                    // 01. Пункт отпр.
                    case 1: return new[] { GetAttribute(1, new Guid("0000004a-0000-0000-0000-000000000000"), route.PlaceRenderGuid, route.FromPointGuid) };

                    // 02. Пункт назн.
                    case 2: return new[] { GetAttribute(2, new Guid("0000004b-0000-0000-0000-000000000000"), route.PlaceRenderGuid, route.ToPointGuid) };

                    // 04. Тип контейнера
                    case 4: return new[] { GetAttribute(4, new Guid("0000004d-0000-0000-0000-000000000000"), route.PlaceRenderGuid, order.TrainTypeGuid) };

                    // 06. Парк контейнера
                    case 64:
                        var owner = (ContOwnerTypeEnum)order.ContOwner;
                        return new[] { GetAttribute(64, new Guid("96cefd54-0f1e-4cf4-8930-dd8ecf3f75ba"), route.PlaceRenderGuid, owner.GetDescription()) };

                    default:
                        return Enumerable.Empty<LtrAttributeObject>();
                }
            }
        }

        private static LtrAttributeObject GetAttribute(int id, Guid attGuid, Guid? placeRender, object value)
        {
            return new LtrAttributeObject()
            {
                ParamType = 5,
                PId = id,
                ParamArrtibGUID = attGuid,
                Value = value == null ? null : value.ToString(),
                PlaceRenderGuid = placeRender,
            };
        }

        private async Task<IEnumerable<IrsGuidAndCNSIGuidDto>> MapGuids(List<GuidsGroup> groups, SaveOrUpdateOrderStoryContext context)
        {
            var stationGuids = new List<Guid?>() { context.PlaceFromGuidCNSI, context.PlaceToGuidCNSI };
            //var etsngGuids = new List<Guid>() { context.ETSNGGuid };
            var clientsGuids = new List<Guid?>() { context.ClientGuidCNSI };
            var trainsTypeGuids = new[] { context.TrainTypeGuidCNSI };

            foreach (var route in context.Routes)
            {
                stationGuids.Add(route.FromPointGuidCNSI);
                stationGuids.Add(route.ToPointGuidCNSI);

                //etsngGuids.AddNotNull(route.Etsng);

                clientsGuids.Add(route.SenderGuidCNSI);
                clientsGuids.Add(route.ReceiverGuidCNSI);
                clientsGuids.Add(route.AgentPortFromCNSI);
                clientsGuids.Add(route.AgentPortToCNSI);
            }

            SetGuidGroupWithCheck(groups, new[] { ReferenceGroupEnum.Station, ReferenceGroupEnum.Port, ReferenceGroupEnum.ForeignPort, ReferenceGroupEnum.GeoPoints }, stationGuids);
            SetGuidGroupWithCheck(groups, new[] { ReferenceGroupEnum.Clients }, clientsGuids);
            SetGuidGroupWithCheck(groups, new[] { ReferenceGroupEnum.TrainsType }, trainsTypeGuids);

            //TODO: Нет в справочниках
            //SetGuidGroup(groups, ReferenceGroupEnum.Etsng, etsngGuids);

            //if (context.GNGGuid.HasValue)
            //{
            //    SetGuidGroup(groups, ReferenceGroupEnum.Gng, new[] { context.GNGGuid.Value });
            //}

            return await _webClient.PostAsync<IEnumerable<IrsGuidAndCNSIGuidDto>>(GET_IRS_GUID_BY_CNSI_GUID, new GetIrsGuidByCNSIGuidContext() { Groups = groups }) ?? Enumerable.Empty<IrsGuidAndCNSIGuidDto>();
        }

        private static void SetGuidGroupWithCheck(List<GuidsGroup> groups, IEnumerable<ReferenceGroupEnum> refGroups, IEnumerable<Guid?> guidList)
        {
            var cleanGuids = guidList
                .Where(x => x.HasValue && x.Value != Guid.Empty)
                .Select(x => x.Value)
                .Distinct()
                .ToArray();

            if (cleanGuids.Any())
            {
                groups.Add(new GuidsGroup()
                {
                    CNSIGuids = cleanGuids,
                    RefGroupIds = refGroups.Select(x => (int)x).ToArray()
                });
            }
        }

        private int GetCurrencyIdByGuid(Guid currencyGuid)
        {
            var list = new Dictionary<Guid, int>();

            list.Add(new Guid("0a35f2ac-bc9e-4062-91f3-35d566c9533e"), 1);
            list.Add(new Guid("9f788e1c-e1db-48c8-bac4-c57465597b63"), 2);

            return list[currencyGuid];
        }
    }

    internal class OrderServiceInfoExt : OrderServiceInfo
    {
        public RouteForSave Route { get; set; }
    }
}
