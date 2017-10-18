namespace Trcont.Ris.Common.Impl
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.TransPicture;

    /*public class NewTransPicService : ITransPicService
    {
        public string GetTransPicString(ITransPicOrder order, IEnumerable<FactByRouteDto> facts)
        {
            order.CheckNull(nameof(order));
            if (order.Routes == null || order.Routes.Count() == 0)
            {
                return string.Empty;
            }

            List<TransPicPart> transPicParts = new List<TransPicPart>();
            // Разбиваем роуты на плечи, которые будут отображены.
            var routes = PrepareRoutes(order.Routes);

            // Разбираем роуты
            foreach (var route in routes)
            {
                var part = GenerateTransPic(order, route, TransPicType.Transit);
                transPicParts.Add(part);
            }

            // сортируем плечи, т.к. роуты не сортированны
            var nparts = ReorderParts(transPicParts, order);

            // Добавляем пункт отправления
            nparts.Insert(0, GetFromPart(order, nparts.First()));
            // Добавляем пункт нзначения
            nparts.Add(GetToPart(order, nparts.Last()));

            if (facts != null && facts.Any())
            {
                SetColors(nparts, order, facts);
            }

            return string.Join("/", nparts);
        }

        private TransPicPart GetFromPart(ITransPicOrder order, TransPicPart part)
        {
            return GenerateTransPic(order, part.Route, TransPicType.Departure);
        }

        private TransPicPart GetToPart(ITransPicOrder order, TransPicPart part)
        {
            return GenerateTransPic(order, part.Route, TransPicType.Delivery);
        }
        
        private IEnumerable<IRoute> PrepareRoutes(IEnumerable<IRoute> orderRoutes)
        {
            List<IRoute> routes = new List<IRoute>();

            foreach (var route in orderRoutes)
            {
                var autoSrv = route.Services.Where(x => RouteHelper.ServiceAuto.Any(y => y == x.RefServiceTypeGuid)).ToList();
                var trainSrv = route.Services.Where(x => RouteHelper.ServiceTrain.Any(y => y == x.RefServiceTypeGuid)).ToList();
                var shipSrv = route.Services.Where(x => RouteHelper.ServiceShipping.Any(y => y == x.RefServiceTypeGuid)).ToList();

                if (autoSrv.Any())
                {
                    routes.Add(CreateRoute(route, autoSrv));
                }
                if (trainSrv.Any())
                {
                    routes.Add(CreateRoute(route, trainSrv));
                }
                if (shipSrv.Any())
                {
                    routes.Add(CreateRoute(route, shipSrv));
                }
            }

            if (!routes.Any())
            {
                routes.Add(CreateRoute(new RouteTrain(), new List<RouteService>()));
            }

            return routes;

        }

        private List<TransPicPart> ReorderParts(List<TransPicPart> transPicParts, ITransPicOrder order)
        {
            List<TransPicPart> parts = new List<TransPicPart>();
            parts.AddRange(transPicParts.Except(parts).Where(x => x.PartType == PartType.aut && x.Route.FromPointGUID == order.PlaceFromGuid));
            parts.AddRange(transPicParts.Except(parts).Where(x => x.PartType != PartType.aut && x.TransPicType == TransPicType.Transit));
            parts.AddRange(transPicParts.Except(parts).Where(x => x.PartType == PartType.aut && x.Route.ToPointGUID == order.PlaceToGuid));
            return parts;
        }

        private RouteTrain CreateRoute(IRoute from, List<RouteService> services)
        {
            RouteTrain nroute = new RouteTrain();
            nroute.ArmIndex = from.ArmIndex;
            nroute.FromPointGUID = from.FromPointGUID;
            nroute.ToPointGUID = from.ToPointGUID;
            nroute.Services = services;
            return nroute;
        }

        private TransPicPart GenerateTransPic(ITransPicOrder order, IRoute route, TransPicType type)
        {
            var part = new TransPicPart();
            part.PartType = GetPartType(order, route, type);
            part.Color = PartColor.b;
            part.Route = route;
            part.TransPicType = type;
            return part;
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

        // много ифов, возможно получится придумать решение лучше
        private PartType GetPartType(ITransPicOrder order, IRoute route, TransPicType type)
        {
            if (route == null)
            {
                return PartType.unk;
            }

            PartType partType;
            if (TransPicType.Departure == type || TransPicType.Delivery == type)
            {
                if (TransPicType.Departure == type && order.PlaceFromPointType == PointTypeEnum.PointPortType)
                {
                    partType = PartType.prt;
                }
                else if (TransPicType.Delivery == type && order.PlaceFromPointType == PointTypeEnum.PointTownType)
                {
                    partType = PartType.trm;
                }
                else if (RouteHelper.ServiceShipping.Any(x => route.Services.Any(y => y.RefServiceTypeGuid == x))
                    || RouteHelper.ServiceAuto.Any(x => route.Services.Any(y => y.RefServiceTypeGuid == x)))
                {
                    partType = PartType.wrh;
                }
                else
                {
                    partType = PartType.sta;
                }
            }
            else if (TransPicType.Transit == type)
            {
                // Проверка на ЖД доставку
                if (RouteHelper.ServiceTrain.Any(x => route.Services.Any(y => y.RefServiceTypeGuid == x)))
                {
                    partType = GetRaPic(order);
                }
                // Проверка на доставку по воде
                else if (RouteHelper.ServiceShipping.Any(x => route.Services.Any(y => y.RefServiceTypeGuid == x)))
                {
                    partType = PartType.fre;
                }
                // проверка на автодоставку
                else if (RouteHelper.ServiceAuto.Any(x => route.Services.Any(y => y.RefServiceTypeGuid == x)))
                {
                    partType = PartType.aut;
                }
                else
                {
                    partType = PartType.unk;
                }
                
            }
            else
            {
                partType = PartType.unk;
            }

            return partType;
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
        
        internal class TransPicPart
        {
            public PartType PartType { get; set; }

            public PartColor Color { get; set; }

            public TransPicType TransPicType { get; set; }

            public IRoute Route { get; set; }

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
    }*/
}
