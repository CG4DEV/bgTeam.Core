namespace Trcont.Cud.Story.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using bgTeam.Web;
    using DapperExtensions.Mapper;
    using Trcont.CitTrans.Service;
    using Trcont.Cud.Common;
    using Trcont.Cud.DataAccess.Dictionaries;
    using Trcont.Cud.DataAccess.Stations;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Entity;
    using Trcont.Cud.Domain.Entity.ExtInfo;
    using Trcont.Cud.Domain.Enum;
    using Trcont.Cud.Domain.Web.Context;
    using Trcont.Cud.Domain.Web.Dto;
    using Trcont.Domain.Entity;

    public class GetOrderInfoByGuidStory : IStory<GetOrderInfoByGuidStoryContext, OrderInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;
        private readonly IWebClient _webClient;
        private readonly ITransPicService _transPicService;

        private static readonly Regex _serviceUSLCode = new Regex(@"((\d+|\.)+)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex _serviceUSLTitle = new Regex(@"(\d+|\.)+\s?((\w|\W)+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private const string GET_REFERENCE_VALUES_ACTION = "api/Info/GetReferenceByIRSGuid";
        private const string GET_FIRMS_INFO_ACTION = "api/Info/GetFirmInfoGuids";
        private const string GET_FACT_TRANSPORT_BYGUID = "api/Info/GetFactTransportInfoByTeoGuid";
        private const string GET_CNSI_GUID_BY_IRS_GUID = "api/Info/GetCNSIGuidByIrsGuid";

        public GetOrderInfoByGuidStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper,
            IWebClient webClient,
            ITransPicService transPicService
            )
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _webClient = webClient;
            _transPicService = transPicService;
        }

        public OrderInfoDto Execute(GetOrderInfoByGuidStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrderInfoDto> ExecuteAsync(GetOrderInfoByGuidStoryContext context)
        {
            OrderInfoDto order;
            IEnumerable<KpServices> services;
            if (!context.IsTeo)
            {
                order = await LoadKPAsync(context.OrderGuid);
                services = await new GetKPServicesByDocumentGuidCmd(_repository).ExecuteAsync(new GetKPServicesByDocumentGuidCmdContext(context.OrderGuid));
            }
            else
            {
                order = await LoadTEOAsync(context.OrderGuid);
                services = await new GetTEOServicesByDocumentGuidCmd(_repository).ExecuteAsync(new GetTEOServicesByDocumentGuidCmdContext(context.OrderGuid));
            }

            if (order == null)
            {
                return null;
            }

            var guids = new List<Guid?>
            {
                order.PlaceFromGuid,
                order.PlaceToGuid,
                order.CountryFromGuid,
                order.CountryToGuid,
                order.CurrencyGuid,
                order.ETSNGGuid,
                order.GNGGuid,
                order.SendingGuid,
                order.TrainTypeGuid,
            };

            guids.AddRange(services.Select(x => x.ParamPlaceFromGuid));
            guids.AddRange(services.Select(x => x.ParamPlaceToGuid));
            guids.AddRange(services.Select(x => x.TerritoryGuid));
            guids.AddRange(services.Select(x => (Guid?)x.TariffCurrencyGuid));

            var refen = await GetCNSIInfoByGuidsAsync(guids);

            await SetFactTransportFieldsAsync(order);

            order.PlaceFromStr = SetFieldStr(order.PlaceFromGuid, refen);
            order.PlaceToStr = SetFieldStr(order.PlaceToGuid, refen);
            order.CountryFromStr = SetFieldStr(order.CountryFromGuid, refen);
            order.CountryToStr = SetFieldStr(order.CountryToGuid, refen);
            order.CurrencyStr = SetFieldStr(order.CurrencyGuid, refen, "{0}");
            order.ETSNGStr = SetFieldStr(order.ETSNGGuid, refen);
            order.GNGStr = SetFieldStr(order.GNGGuid, refen);
            order.SendingStr = SetFieldStr(order.SendingGuid, refen, "{0}");
            order.TrainTypeStr = SetFieldStr(order.TrainTypeGuid, refen, "{0}");

            order.PlaceFromCNSIGuid = SetFieldCNSIId(order.PlaceFromGuid, refen);
            order.PlaceToCNSIGuid = SetFieldCNSIId(order.PlaceToGuid, refen);
            order.CountryFromCNSIGuid = SetFieldCNSIId(order.CountryFromGuid, refen);
            order.CountryToCNSIGuid = SetFieldCNSIId(order.CountryToGuid, refen);
            order.CurrencyCNSIGuid = SetFieldCNSIId(order.CurrencyGuid, refen);
            order.ETSNGCNSIGuid = SetFieldCNSIId(order.ETSNGGuid, refen);
            order.GNGCNSIGuid = SetFieldCNSIId(order.GNGGuid, refen);
            order.SendingCNSIGuid = SetFieldCNSIId(order.SendingGuid, refen);
            order.TrainTypeCNSIGuid = SetFieldCNSIId(order.TrainTypeGuid, refen);

            var routes = services.Select(x => new RouteDto(x.ParamPlaceFromGuid, x.ParamPlaceToGuid)).Distinct().ToArray();
            var executers = await GetIRSReferenceValuesAsync(services);
            var currencies = await GetCurrenciesInfoAsync(services);
            var pointTypes = await GetPointTypesAsync(new Guid[] { order.PlaceFromGuid, order.PlaceToGuid });

            order.PlaceFromPointType = SetPointTypeField(order.PlaceFromGuid, pointTypes);
            order.PlaceToPointType = SetPointTypeField(order.PlaceToGuid, pointTypes);

            var firms = await GetFirmsInfoAsync(services);
            var collection = new RouteCollection();

            var number = 1;
            foreach (var route in routes)
            {
                var newRoute = _mapper.Map(route, new RouteTrain());

                newRoute.Number = number;

                number++;

                newRoute.FromPointTitle = SetFieldStr(route.From, refen);
                newRoute.ToPointTitle = SetFieldStr(route.To, refen);

                //newRoute.FromPointCNSIId = SetFieldCNSIId(route.From, refen);
                //newRoute.ToPointCNSIId = SetFieldCNSIId(route.To, refen);

                newRoute.Services = services.Where(x => x.ParamPlaceFromGuid == route.From && x.ParamPlaceToGuid == route.To)
                    .Select(x =>
                    {
                        var srv = _mapper.Map(x, new RouteService());

                        if (x.ExecGuid.HasValue && executers != null)
                        {
                            var executer = executers.SingleOrDefault(y => y.ReferenceGuid == x.ExecGuid);
                            srv.RzdCargoSender = executer?.Value;
                        }

                        srv.Code = GetServiceCode(x.ServiceTitle);
                        srv.CurrencyCode = currencies.SingleOrDefault(y => y.ReferenceGuid == x.TariffCurrencyGuid)?.IntCurrencyCode;
                        srv.CurrencyTitle = currencies.SingleOrDefault(y => y.ReferenceGuid == x.TariffCurrencyGuid)?.CurrencyTitle;
                        srv.Title = GetServiceTitle(x.ServiceTitle);

                        srv.CargoSender = GetFirmName(firms, srv.ParamSenderGuid);
                        srv.CargoReciever = GetFirmName(firms, srv.ParamReceiverGuid);
                        srv.CargoSender2 = GetFirmName(firms, srv.ParamSender2Guid);
                        srv.CargoReciever2 = GetFirmName(firms, srv.ParamReceiver2Guid);
                        srv.PortFromAgent = GetFirmName(firms, srv.ParamPortFromAgentGuid);
                        srv.PortToAgent = GetFirmName(firms, srv.ParamPortToAgentGuid);

                        srv.Summ = srv.Summ.RoundUp(2);
                        srv.SummVAT = srv.SummVAT.RoundUp(2);
                        srv.Tariff = srv.Tariff.RoundUp(2);
                        srv.TariffVAT = srv.TariffVAT.RoundUp(0);
                        return srv;
                    })
                    .ToArray();

                newRoute.Name = GetRouteName(newRoute.Services);

                collection.Add(newRoute);
            }

            DislocationStation dislocation = null;

            //if (context.IsTeo)
            //{
            //    var cmd = new GetContainerDislockCmd(_logger, _mapper, _repository, _client);
            //    dislocation = await cmd.ExecuteAsync(new GetContainerDislockCmdContext { OrderNumber = order.Number.ToString() });
            //}

            order.Routes = collection;
            order.TransPic = _transPicService.GetTransPicString(order, dislocation);

            order.Summ = order.Summ.RoundUp(2);
            return order;
        }

        private string GetRouteName(IList<RouteService> services)
        {
            return null;
        }

        private async Task<IEnumerable<IrsGuidAndCNSIGuidDto>> GetCNSIInfoByGuidsAsync(List<Guid?> guids)
        {
            var cleanGuids = guids.Where(x => x.HasValue).Select(x => x.Value).Distinct().ToArray();
            return await _webClient.PostAsync<IEnumerable<IrsGuidAndCNSIGuidDto>>(GET_CNSI_GUID_BY_IRS_GUID,
                new GetCNSIGuidByIrsGuidContext() { IrsGuids = cleanGuids })
                ?? Enumerable.Empty<IrsGuidAndCNSIGuidDto>();
        }

        private async Task SetFactTransportFieldsAsync(OrderInfoDto order)
        {
            if (!order.IsTeo)
            {
                return;
            }

            var facts = await _webClient
                .PostAsync<IEnumerable<FactResponse>>(GET_FACT_TRANSPORT_BYGUID, new { ReferenceGuids = new[] { order.ReferenceGuid } }) ?? Enumerable.Empty<FactResponse>();

            var fact = facts.FirstOrDefault();
            if (fact == null)
            {
                return;
            }

            var factInfo = fact.FactTransportCollection.FirstOrDefault(x => x.FactSourceId == (int)FactSourceEnum.Doc14);
            if (factInfo == null)
            {
                return;
            }

            order.ArrivalDate = factInfo.ComDate;
            order.SendDate = factInfo.OutDate;
        }

        private async Task<IEnumerable<PointTypeDto>> GetPointTypesAsync(IEnumerable<Guid> placeGuids)
        {
            var cmd = new GetPointTypesCmd(_repository);
            return await cmd.ExecuteAsync(new GetPointTypesCmdContext() { PlaceGuids = placeGuids });
        }

        private string GetFirmName(IEnumerable<FirmInfoDto> firms, Guid? firmGuid)
        {
            if (!firmGuid.HasValue)
            {
                return null;
            }

            return firms.SingleOrDefault(x => x.ReferenceGUID == firmGuid.Value)?.Name;
        }

        private async Task<IEnumerable<FirmInfoDto>> GetFirmsInfoAsync(IEnumerable<KpServices> services)
        {
            List<Guid> clientGuids = new List<Guid>();

            foreach (var service in services)
            {
                clientGuids.AddNotNull(service.ParamSenderGuid);
                clientGuids.AddNotNull(service.ParamSender2Guid);
                clientGuids.AddNotNull(service.ParamReceiverGuid);
                clientGuids.AddNotNull(service.ParamReceiver2Guid);
                clientGuids.AddNotNull(service.ParamPortFromAgentGuid);
                clientGuids.AddNotNull(service.ParamPortToAgentGuid);
            }

            clientGuids = clientGuids.Distinct().ToList();

            if (!clientGuids.Any())
            {
                return Enumerable.Empty<FirmInfoDto>();
            }

            return (await _webClient.PostAsync<List<FirmInfoDto>>(GET_FIRMS_INFO_ACTION, new GetFirmsInfoByGuidsContext { ClientGuids = clientGuids })) ?? Enumerable.Empty<FirmInfoDto>();
        }

        private async Task<IEnumerable<CurrencyInfoDto>> GetCurrenciesInfoAsync(IEnumerable<KpServices> services)
        {
            var currencyGuids = services.Select(x => x.TariffCurrencyGuid).Distinct().ToArray();
            if (!currencyGuids.Any())
            {
                return Enumerable.Empty<CurrencyInfoDto>();
            }

            var sql = @"SELECT 
                            r.ReferenceGUID,
                            r.ReferenceAccount as CurrencyCode,
                            r.ReferenceTitle as CurrencyTitle
                        FROM dbo.Reference r WHERE r.RefTypeId = 16 AND r.ReferenceGUID IN (SELECT id FROM @ReferenceIds)";
            return await _repository.GetAllAsync<CurrencyInfoDto>(sql, new { ReferenceIds = new GuidDbType(currencyGuids) });
        }

        private string GetServiceTitle(string serviceTitle)
        {
            return GetByRegex(serviceTitle, _serviceUSLTitle, 2);
        }

        private string GetServiceCode(string serviceTitle)
        {
            return GetByRegex(serviceTitle, _serviceUSLCode, 1);
        }

        private static string GetByRegex(string input, Regex pattern, int groupIndex)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return null;
            }

            var match = pattern.Match(input);
            if (match.Success)
            {
                return match.Groups[groupIndex].Value;
            }

            return null;
        }

        private async Task<IEnumerable<ReferenceValue>> GetIRSReferenceValuesAsync(IEnumerable<KpServices> services)
        {
            var execGuids = services.Where(x => x.ExecGuid.HasValue).Select(x => x.ExecGuid.Value).Distinct().ToArray();
            if (!execGuids.Any())
            {
                return Enumerable.Empty<ReferenceValue>();
            }

            return await _webClient.PostAsync<List<ReferenceValue>>(GET_REFERENCE_VALUES_ACTION, new GetReferenceValuesByGuidsContext { ReferenceGuids = execGuids });
        }

        private async Task<OrderInfoDto> LoadKPAsync(Guid orderGuid)
        {
            var sql = @"
    SELECT 
         d.ReferenceGUID,
         d.Number,                      -- Номер
         d.TrainTypeGUID,               -- Тип контейнера
         d.DocumentTitle,               -- Маршрут
         d.ContractGUID,                -- Идентификатор договора
         d.ClientGUID,                  -- Идентификатор клиента
         d.CreateDate,                  -- Дата создания
--       ????????????                   -- Ранг отправки
                                        -- Дата погрузки
         (SELECT MIN(ks.LoadDate) FROM KPServices ks WHERE ks.RefDocumentGUID = d.ReferenceGUID AND ks.IsActive = 1) as LoadDate, 
         d.Status as Status,            -- Статус
         d.ContrRelationsType,          -- Тип договорных отношений
         d.PeriodBeginDate,             -- Период начала исполнения заявки
         d.PeriodEndDate,               -- Период конца исполнения заявки
         
         d.ContactFace,                 -- Контактное лицо
         d.ContactPhone,                -- Контактное лицо телефон (c.FPhone)

         d.ETSNGGUID,                   -- ЕТСНГ
         d.GNGGUID,                     -- ГНГ
         d.Weight,                      -- Вес груза (т) в ваг./конт
         d.WeightBrutto,                -- Вес брутто груза контейнера
--       ????????????                   -- Вид упаковки
--       ????????????                   -- Доп. информация

         d.CurrencyGUID,                -- Валюта оценки стоимости

         d.OutCategory,                 -- Тип отправки
         d.SendingGUID,                 -- Признак отправки
         
         d.ContainerQuantity,           -- Количество контейнеров
         d.ContOwner,                   -- Принадлежность контейнера
         d.WagonPark,                   -- Принадлежность вагона
         d.Speed,                       -- Скорость
         d.CustomType,                  -- Таможенный режим

         d.PlaceFromGUID,               -- Пункт отправления
         d.CountryFromGUID,             -- Страна отправления
         d.PlaceToGUID,                 -- Пункт назначения
         d.CountryToGUID,               -- Страна назначения
                                        -- Общая сумма НДС, руб. (все плечи)
         (SELECT SUM(ks.Summ) FROM KPServices ks WHERE ks.RefDocumentGUID = d.ReferenceGUID AND ks.IsActive = 1) as Summ,
         0 as IsTeo
    FROM KPDocuments d
    WHERE d.ReferenceGUID = @OrderGuid";
            return await _repository.GetAsync<OrderInfoDto>(sql, new { OrderGuid = orderGuid });
        }

        private async Task<OrderInfoDto> LoadTEOAsync(Guid orderGuid)
        {
            var sql = @"
    SELECT 
         t.ReferenceGUID,
         t.Number,                      -- Номер
         t.TrainTypeGUID,               -- Тип контейнера
         t.DocumentTitle,               -- Маршрут
         t.ContractGUID,                -- Идентификатор договора
         t.ClientGUID,                  -- Идентификатор клиента
         t.CreateDate,                  -- Дата создания
--       ????????????                   -- Ранг отправки

                                        -- Дата погрузки
         (SELECT MIN(ts.LoadDate) FROM TeoServices ts WHERE ts.RefDocumentGUID = t.ReferenceGUID) as LoadDate, 
         t.Status + 100 as Status,      -- Статус
         d.ContrRelationsType,          -- Тип договорных отношений
         t.PeriodBeginDate,             -- Период начала исполнения заявки
         t.PeriodEndDate,               -- Период конца исполнения заявки
         
--       d.ContactFace,                 -- Контактное лицо
--       d.ContactPhone,                -- Контактное лицо телефон (c.FPhone)
--       c.FMobilePhone,                -- Контактное лицо доп. телефон

         t.ETSNGGUID,                   -- ЕТСНГ
         t.GNGGUID,                     -- ГНГ
         t.Weight,                      -- Вес груза (т) в ваг./конт
         t.WeightBrutto,                -- Вес брутто груза контейнера
--       ????????????                   -- Вид упаковки
--       ????????????                   -- Доп. информация

         t.CurrencyGUID,                -- Валюта оценки стоимости

         t.OutCategory,                 -- Тип отправки
         t.SendingGUID,                 -- Признак отправки
         
         t.ContainerQuantity,           -- Количество контейнеров
         t.ContOwner,                   -- Принадлежность контейнера
         t.WagonPark,                   -- Принадлежность вагона
         t.Speed,                       -- Скорость
         t.CustomType,                  -- Таможенный режим

         t.PlaceFromGUID,               -- Пункт отправления
         t.CountryFromGUID,             -- Страна отправления
         t.PlaceToGUID,                 -- Пункт назначения
         t.CountryToGUID,               -- Страна назначения
                                        -- Общая сумма НДС, руб. (все плечи)
         (SELECT SUM(ts.Summ) FROM TeoServices ts WHERE ts.RefDocumentGUID = t.ReferenceGUID) as Summ,
         1 as IsTeo
  FROM TEODocuments t
  LEFT JOIN KPDocuments d ON t.ReferenceGUID = d.TEOGUID
  WHERE t.ReferenceGUID = @OrderGuid";

            return await _repository.GetAsync<OrderInfoDto>(sql, new { OrderGuid = orderGuid });
        }

        private string SetFieldStr(Guid? guid, IEnumerable<IrsGuidAndCNSIGuidDto> refen, string format = "{1} : {0}")
        {
            if (guid.HasValue)
            {
                var value = refen.FirstOrDefault(x => x.ReferenceGuid == guid);
                if (value != null)
                {
                    return string.Format(format, value.Title, value.Account);
                }
            }

            return null;
        }

        private Guid? SetFieldCNSIId(Guid? guid, IEnumerable<IrsGuidAndCNSIGuidDto> refen)
        {
            if (guid.HasValue)
            {
                var value = refen.FirstOrDefault(x => x.ReferenceGuid == guid);
                if (value != null)
                {
                    return value.CNSIGuid;
                }
            }

            return null;
        }

        private PointTypeEnum SetPointTypeField(Guid guid, IEnumerable<PointTypeDto> refen)
        {
            if (guid != Guid.Empty)
            {
                var value = refen.FirstOrDefault(x => x.PlaceGuid == guid);
                if (value != null)
                {
                    return value.PointType;
                }
            }

            return PointTypeEnum.PointPortType;
        }
    }
}
