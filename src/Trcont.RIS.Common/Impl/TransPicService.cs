namespace Trcont.Ris.Common.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using bgTeam.Extensions;
    using Trcont.Common.Utils;
    using Trcont.Ris.DataAccess.Helpers;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Enums;
    using Trcont.Ris.Domain.TransPicture;

    public class TransPicService : ITransPicService
    {
        public string GetTransPicString(ITransPicOrder order, IEnumerable<FactByRouteDto> facts)
        {
            order.CheckNull(nameof(order));
            if (order.Routes.NullOrEmpty())
            {
                return string.Empty;
            }

            List<TransPicPart> transPicParts = new List<TransPicPart>();
            // Разбираем роуты
            foreach (var route in order.Routes)
            {
                var part = GenerateTransPic(order, route, TransPicType.Transit);
                transPicParts.Add(part);
            }

            // Добавляем пункт отправления
            var begin = transPicParts.First();
            if (begin.Route.RouteType != RouteTypeEnum.INTRMDL)
            {
                transPicParts.Insert(0, GetFromPart(order, begin));
            }

            // Добавляем пункт нзначения
            var end = transPicParts.Last();
            if (end.Route.RouteType != RouteTypeEnum.INTRMDL)
            {
                transPicParts.Add(GetToPart(order, end));
            }

            if (facts != null && facts.Any())
            {
                SetColors(transPicParts, order, facts);
            }

            return string.Join("/", transPicParts);
        }

        private TransPicPart GetFromPart(ITransPicOrder order, TransPicPart part)
        {
            return GenerateTransPic(order, part.Route, TransPicType.Departure);
        }

        private TransPicPart GetToPart(ITransPicOrder order, TransPicPart part)
        {
            return GenerateTransPic(order, part.Route, TransPicType.Delivery);
        }

        private TransPicPart GenerateTransPic(ITransPicOrder order, OrderRouteDto route, TransPicType type)
        {
            var part = new TransPicPart();
            part.PartType = GetPartType(order, route.RouteType, type);
            part.Color = PartColor.b;
            part.Route = route;
            part.TransPicType = type;
            return part;
        }

        private PartType GetPartType(ITransPicOrder order, RouteTypeEnum routeType, TransPicType type)
        {
            if (TransPicType.Departure == type || TransPicType.Delivery == type)
            {
                if (routeType == RouteTypeEnum.JUNCTION || routeType == RouteTypeEnum.RAIL)
                {
                    return PartType.sta;
                }
                else if (routeType == RouteTypeEnum.TL)
                {
                    return PartType.wrh;
                }
                else if (routeType == RouteTypeEnum.VESSEL)
                {
                    return PartType.prt;
                }

                return PartType.unk;
            }

            switch (routeType)
            {
                case RouteTypeEnum.TL:
                    {
                        return PartType.aut;
                    }
                case RouteTypeEnum.INTRMDL:
                    {
                        return PartType.trm;
                    }
                case RouteTypeEnum.RAIL:
                    {
                        return GetRaPic(order);
                    }
                case RouteTypeEnum.VESSEL:
                    {
                        return PartType.fre;
                    }
                case RouteTypeEnum.JUNCTION:
                    {
                        return GetRaPic(order);
                    }
                default:
                    {
                        throw new Exception($"Неизвестный тип плеча {routeType}");
                    }
            }
        }

        private PartType GetRaPic(ITransPicOrder order)
        {
            if (order.TrainTypeGuid != Guid.Empty)
            {
                if (RouteHelper.ContainerSmall.Contains(order.TrainTypeGuid))
                {
                    return PartType.ra2;
                }

                if (RouteHelper.ContainerBig.Contains(order.TrainTypeGuid))
                {
                    return PartType.ra4;
                }
            }

            return PartType.unk;
        }

        private static void SetColors(List<TransPicPart> parts, ITransPicOrder order, IEnumerable<FactByRouteDto> facts)
        {
            // Если не ТЕО документ, то не выставляем цвета
            if (!order.TeoId.HasValue)
            {
                return;
            }

            foreach (var part in parts)
            {
                var curFacts = facts.Where(x => x.FromPointGuid == part.Route.FromPointGUID && x.ToPointGuid == part.Route.ToPointGUID);
                if (curFacts.Any(x => x.ArrivalDate.HasValue))
                {
                    if (curFacts.Any(x => !x.ArrivalDate.HasValue))
                    {
                        part.Color = PartColor.y;
                        return;
                    }
                    else
                    {
                        part.Color = PartColor.g;
                    }
                }
                else
                {
                    return;
                }
            }
        }
    }

    internal class TransPicPart
    {
        public PartType PartType { get; set; }

        public PartColor Color { get; set; }

        public TransPicType TransPicType { get; set; }

        public OrderRouteDto Route { get; set; }

        public override string ToString()
        {
            return $"{PartType.ToString()},{Color.ToString()}";
        }
    }

    internal enum PartType
    {
        /// <summary>
        /// ЖД станция
        /// </summary>
        sta,

        /// <summary>
        /// Склад
        /// </summary>
        wrh,

        /// <summary>
        /// ЖД перевозка 20ф
        /// </summary>
        ra2,

        /// <summary>
        /// ЖД перевозка 40ф
        /// </summary>
        ra4,

        /// <summary>
        /// Автоперевозка
        /// </summary>
        aut,

        /// <summary>
        /// Нераспознанное плечо
        /// </summary>
        unk,

        /// <summary>
        /// Фрахт
        /// </summary>
        fre,

        /// <summary>
        /// Порт
        /// </summary>
        prt,

        /// <summary>
        /// Терминал
        /// </summary>
        trm
    }

    internal enum PartColor
    {
        /// <summary>
        /// Жёлтый
        /// </summary>
        y,

        /// <summary>
        /// Зелёный
        /// </summary>
        g,

        /// <summary>
        /// Синий
        /// </summary>
        b
    }

    internal enum TransPicType
    {
        /// <summary>
        /// Пункт отправки
        /// </summary>
        Departure,

        /// <summary>
        /// Пункт доставки
        /// </summary>
        Delivery,

        /// <summary>
        /// Плечо. Переходной пункт
        /// </summary>
        Transit
    }
}
