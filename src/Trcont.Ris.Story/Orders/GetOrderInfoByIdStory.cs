namespace Trcont.Ris.Story.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Trcont.Common.Utils;
    using Trcont.Ris.DataAccess.Order;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;

    public class GetOrderInfoByIdStory : IStory<GetOrderInfoByIdStoryContext, OrderInfoDto>
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IMapperBase _mapper;

        private static readonly Regex _serviceUSLTitle = new Regex(@"(\d+|\.)+\s?((\w|\W)+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        public GetOrderInfoByIdStory(
            IAppLogger logger,
            IRepository repository,
            IMapperBase mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public OrderInfoDto Execute(GetOrderInfoByIdStoryContext context)
        {
            return ExecuteAsync(context).Result;
        }

        public async Task<OrderInfoDto> ExecuteAsync(GetOrderInfoByIdStoryContext context)
        {
            IEnumerable<OrderRouteDto> routes;
            IEnumerable<OrderServicesDto> services;
            IEnumerable<OrderServiceParamsDto> serviceParams;
            OrderInfoDto order = await LoadOrderAsync(context.Id);

            if (order == null)
            {
                return null;
            }

            if (order.TeoId.HasValue)
            {
                var routesCmd = new GetOrderRoutesByTeoIdCmd(_repository);
                routes = await routesCmd.ExecuteAsync(new GetOrderRoutesByTeoIdCmdContext()
                {
                    TeoId = order.TeoId.Value
                });
                var cmd = new GetOrderServicesByDocumentIdCmd(_repository);
                services = await cmd.ExecuteAsync(new GetOrderServicesByDocumentIdCmdContext()
                {
                    OrderId = context.Id
                });
                var paramCmd = new GetOrderServicesParamsByDocumentIdCmd(_repository);
                serviceParams = await paramCmd.ExecuteAsync(new GetOrderServicesParamsByDocumentIdCmdContext()
                {
                    OrderId = context.Id,
                    ServiceIds = services.Select(x => x.ServiceId).ToArray()
                });
            }
            else
            {
                /*var cmd = new GetKPServicesByDocumentIdCmd(_repository);
                services = await cmd.ExecuteAsync(new GetKPServicesByDocumentIdCmdContext()
                {
                    OrderId = context.Id
                });
                var paramCmd = new GetKPServicesParamsByDocumentIdCmd(_repository);
                serviceParams = await paramCmd.ExecuteAsync(new GetKPServicesParamsByDocumentIdCmdContext()
                {
                    OrderId = context.Id,
                    ServiceIds = services.Select(x => x.ServiceId).ToArray()
                });*/

                throw new Exception($"У переданного КП с идентификатором {order.Id} нет заказа");
            }

            order.Summ = services.Sum(x => x.Summ);
            foreach (var route in routes)
            {
                var currentServices = services.Where(x => x.ArmIndex == route.ArmIndex);
                route.ServiceParams = serviceParams
                    .Where(s => currentServices.Any(x => x.ServiceId == s.OrderServiceId || x.ServiceId == s.TeoServiceId))
                    .DistinctBy(x => x.AttribGuid);

                route.Services = currentServices
                    .Select(x =>
                    {
                        var srv = _mapper.Map(x, new RouteService());

                        srv.Title = GetServiceTitle(x.ServiceTitle);

                        srv.Summ = srv.Summ.RoundUp(2);
                        srv.SummVAT = srv.SummVAT.RoundUp(2);
                        srv.Tariff = srv.Tariff.RoundUp(2);
                        srv.TariffVAT = srv.TariffVAT.RoundUp(0);

                        return srv;
                    })
                    .ToArray();
            }

            order.Routes = routes;
            order.Summ = order.Summ.RoundUp(2);
            order.DocumentTitle = GetDocumentTitle(routes);

            return order;
        }

        private string GetDocumentTitle(IEnumerable<OrderRouteDto> routes)
        {
            var fromCn = routes.FirstOrDefault()?.FromCountryTitle;
            var toCn = routes.LastOrDefault()?.ToCountryTitle;

            var strFrom = !string.IsNullOrEmpty(fromCn) ? $"{fromCn}, " : null;
            var strTo = !string.IsNullOrEmpty(toCn) ? $"{toCn}, " : null;

            return $"{strFrom}{routes.FirstOrDefault()?.FromPointTitle} - {strTo}{routes.LastOrDefault()?.ToPointTitle}";
        }

        private string GetServiceTitle(string serviceTitle)
        {
            return GetByRegex(serviceTitle, _serviceUSLTitle, 2);
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

        private async Task<OrderInfoDto> LoadOrderAsync(int id)
        {
            var sql = @"
            SELECT 
                o.Id,
                o.IrsGuid,
                o.TeoId,
                o.TeoGuid,
                o.Number,                        -- Номер
                o.TrainTypeId,                   -- Тип контейнера
                ct.IrsGuid AS TrainTypeGuid,
                ct.Title AS TrainTypeTitle,
                ct.CNSICode AS TrainTypeCNSICode,
                ct.CNSIGuid AS TrainTypeCNSIGuid,
                o.DocumentTitle,                 -- Маршрут
                o.ContractId,                    -- Идентификатор договора
                contr.IrsGuid AS ContractGuid,
                o.ClientId,                      -- Идентификатор клиента
                c.ClientGUID,
                o.OrderDate,                    -- Дата создания
                o.StatusId,                      -- Статус
                st.Name AS StatusTitle,
                o.ContrRelationsTypeId,          -- Тип договорных отношений
                crt.Name AS ContrRelationsTypeTitle,
                o.PeriodBeginDate,               -- Период начала исполнения заявки
                o.PeriodBeginOffset,             -- Смещение
                o.PeriodEndDate,                 -- Период конца исполнения заявки       
                o.PeriodEndOffset,             -- Смещение
                --o.ContactFace,                 -- Контактное лицо
                --o.ContactPhone,                -- Контактное лицо телефон (c.FPhone)
                o.ETSNGId,                       -- ЕТСНГ
                e.Title AS ETSNGTitle,
                e.Code AS ETSNGCode, 
                o.GNGId,                         -- ГНГ
                gng.Title AS GNGTitle,
                gng.Code AS GNGCode,
                o.Weight,                        -- Вес груза (т) в ваг./конт
                o.WeightBrutto,                  -- Вес брутто груза контейнера
                o.CurrencyId,                    -- Валюта оценки стоимости
                cur.Title AS CurrencyTitle,
                o.OutCategory,                   -- Тип отправки
                o.SendTypeId,                    -- Признак отправки   
                sendtype.Name AS SendTypeTitle,
                o.ContainerQuantity,             -- Количество контейнеров
                o.ContOwner,                     -- Принадлежность контейнера
                o.WagonPark,                     -- Принадлежность вагона
                o.Speed,                         -- Скорость
                o.CustomType,                    -- Таможенный режим
                o.PlaceFromId,                   -- Пункт отправления
                placeFrom.IrsGuid AS PlaceFromGuid,
                placeFrom.Title AS PlaceFromTitle,
                placeFrom.CnsiCode AS PlaceFromCnsiCode,
                placeFrom.CnsiGuid AS PlaceFromCnsiGuid,
                placeFrom.PointType AS PlaceFromPointType,
                placeFrom.CountryId AS CountryFromId,             -- Страна отправления
                countryFrom.Title AS CountryFromTitle, 
                o.PlaceToId,                     -- Пункт назначения
                placeTo.IrsGuid AS PlaceToGuid,
                placeTo.Title AS PlaceToTitle,
                placeTo.CnsiCode AS PlaceToCnsiCode,
                placeTo.CnsiGuid AS PlaceToCnsiGuid,
                placeTo.PointType AS PlaceToPointType,
                placeTo.CountryId AS CountryToId,               -- Страна назначения
                countryTo.Title AS CountryToTitle,
                o.TransTypeId,
                tt.Name AS TransTypeTitle,
                fct.PlanArrivalDate,
                fct.TransDate AS SendDate,
                fct.ArrivalDate,
                o.CreateDate,
                o.TimeStamp,
                fct.LoadDate
        FROM Orders o WITH (NOLOCK)
            LEFT JOIN vPoints placeFrom WITH (NOLOCK) ON o.PlaceFromId = placeFrom.Id
            LEFT JOIN vPoints placeTo WITH (NOLOCK) ON o.PlaceToId = placeTo.Id
            LEFT JOIN Country countryFrom WITH (NOLOCK) ON placeFrom.CountryId = countryFrom.Id
            LEFT JOIN Country countryTo WITH (NOLOCK) ON placeTo.CountryId = countryTo.Id
            LEFT JOIN Currency cur WITH (NOLOCK) ON o.CurrencyId = cur.Id
            LEFT JOIN GNG gng WITH (NOLOCK) ON o.GngId = gng.Id
            LEFT JOIN ETSNG e WITH (NOLOCK) ON o.EtsngId = e.Id
            LEFT JOIN ContainerType ct WITH (NOLOCK) ON o.TrainTypeId = ct.Id
            LEFT JOIN OrdersStatus st WITH (NOLOCK) ON st.Id = o.StatusId
            LEFT JOIN vClients c WITH (NOLOCK) ON c.Id = o.ClientId
            LEFT JOIN Contract contr WITH (NOLOCK) ON contr.Id = o.ContractId
            LEFT JOIN ContrRelationsType crt WITH (NOLOCK) ON crt.Id = o.ContrRelationsTypeId
            LEFT JOIN SendType sendtype WITH (NOLOCK) ON sendtype.Id = o.SendTypeId
            LEFT JOIN TransType tt WITH (NOLOCK) ON tt.Id = o.TransTypeId
            LEFT JOIN 
            (
            SELECT fct1.OrderId, 
                MAX(fct1.LoadDate) AS LoadDate, 
                MAX(fct1.PlanArrivalDate) AS PlanArrivalDate,
                MAX(fct1.TransDate) AS TransDate,
                MAX(fct1.ArrivalDate) AS ArrivalDate
            FROM OrdersFact fct1 
            WHERE fct1.FactSourceId = 304686
            GROUP BY fct1.OrderId) fct ON fct.OrderId = o.TeoId
        WHERE o.Id = @OrderId";

            return await _repository.GetAsync<OrderInfoDto>(sql, new { OrderId = id });
        }
    }
}
