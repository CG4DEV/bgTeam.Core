namespace Trcont.Ris.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Trcont.App.Service.Entity;
    using Trcont.Ris.DataAccess.Common;
    using Trcont.Ris.DataAccess.Services;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public interface IOrderServiceService
    {
        Task FillAttributesAsync(OrderInfo order, List<OrderServiceInfoExt> orderService);
    }

    public class OrderServiceService : IOrderServiceService
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;

        public OrderServiceService(
            IAppLogger logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task FillAttributesAsync(OrderInfo order, List<OrderServiceInfoExt> orderService)
        {
            // Навешиваем атрибуты на услуги
            var massGuid = orderService
                .Select(x => x.ServiceId)
                .ToArray();

            var serviceMap = await new GetParamsForServicesCmd(_repository)
                .ExecuteAsync(new GetParamsForServicesCmdContext() { ServiceIds = massGuid });

            //var paramList = new RouteServiceParamList();

            foreach (var item in orderService)
            {
                var list2 = serviceMap.Where(x => x.ServiceGuid == item.ServiceTypeGuid);

                var attMass = list2
                    .SelectMany(x => GetAttribute(x, item, order))
                    .Where(x => x != null)
                    .ToArray();

                item.Attributes2 = attMass;

                //var priceService = await new GetServicesByParamsCmd(_logger, _repository)
                //    .ExecuteAsync(new GetServicesByParamsCmdContext() { ServiceId = item.ServiceTypeId, Attributes = attMass });

                //if (priceService != null)
                //{
                //    item.SourcePriceServiceId = priceService.Id;
                //    item.SourceReferenceId = priceService.PriceId;
                //    //item.ServiceForPoint = true;
                //    //item.Tariff = priceService.Rate;
                //    //item.TariffVAT = priceService.RateVAT;
                //    //item.TariffType = priceService.TarifType;
                //}
            }
        }

        private IEnumerable<OrderServiceAttribute> GetAttribute(ServiceParam param, OrderServiceInfoExt serviceInfo, OrderInfo order)
        {
            var paramList = new RouteServiceParamList();
            var route = serviceInfo.Route;

            if (paramList.RouteServiceParam.ContainsKey(param.Code))
            {
                var attrParam = paramList.RouteServiceParam[param.Code];
                var value = attrParam.Property.GetValue(route, null);
                OrderServiceAttribute attr = CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, value, attrParam.Id == 3);

                return new[] { attr };
            }
            else
            {
                switch (param.Code)
                {
                    // 01. Пункт отпр.
                    case 1: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, route.FromPointGuid, true) };

                    // 02. Пункт назн.
                    case 2: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, route.ToPointGuid, true) };

                    // 04. Тип контейнера
                    case 4: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, order.TrainTypeGuid, true) };

                    // 31. Доп. условие автоперевозки
                    case 27: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, "Без дополнительных условий", true) };

                    // 06. Парк контейнера
                    case 64:
                        var cowner = (ContOwnerTypeEnum) order.ContOwner;
                        return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, Guid.Parse(cowner.GetDescription()), true) };

                    //13. Парк вагона
                    case 19:
                        var wowner = (WagonParkTypeEnum)order.WagonPark;
                        return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, Guid.Parse(wowner.GetDescription()), true) };

                    //15. Масса брутто
                    case 67: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, order.WeightBrutto, true) };

                    //16. Вид сообщения
                    case 74: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, order.TransTypeGuid, true) };

                    //18. Единица измерения
                    case 81: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, serviceInfo.TariffGuid, true) };

                    //22.Код груза ЕТСНГ
                    case 85: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, order.EtsngGuid, true) };

                    //23. Вид отправки
                    case 123:
                        // A527AC31-2BCA-45B9-839E-4ECE713683F8 - Вагонная
                        // 375C5B17-2F19-4A0E-843A-5D248C40E60E - Контейнерная
                        var value = order.OutCategory == 1 ? new Guid("375c5b17-2f19-4a0e-843a-5d248c40e60e") : new Guid("a527ac31-2bca-45b9-839e-4ece713683f8");
                        return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, value, true) };

                    // 28. Тип комплекса
                    case 128: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, "Базовый", true) };

                    // 29. Терминал оказания услуги
                    case 129: return new[] { CreateAttribute(param.Code, param.ParamGuid, route.PlaceRenderGuid, "Пустое значение", true) };

                    default: return Enumerable.Empty<OrderServiceAttribute>();
                }
            }
        }

        private static OrderServiceAttribute CreateAttribute(int id, Guid attGuid, Guid? placeRender, object value, bool isRequired = false)
        {
            return new OrderServiceAttribute()
            {
                AttId = id,
                AttGuid = attGuid,
                AttType = 5,
                Value = value == null ? null : value.ToString(),
                PlaceRender = placeRender,
                IsRequired = isRequired
            };
        }
    }
}
