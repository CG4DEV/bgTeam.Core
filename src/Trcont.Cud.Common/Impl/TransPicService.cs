namespace Trcont.Cud.Common.Impl
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Cud.Domain;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Entity.ExtInfo;
    using Trcont.Cud.Domain.Enum;

    public class TransPicService : ITransPicService
    {
        private static readonly Guid[] _containerSmall = GetSmallContTypes();
        private static readonly Guid[] _containerBig = GetBigContTypes();

        private static readonly PointTypeEnum[] _pointStationTypeCondition = new PointTypeEnum[]
            {
                PointTypeEnum.PointStationType,
                PointTypeEnum.PointStationType2
            };

        private static Guid[] GetSmallContTypes()
        {
            return GetContTypes("Small");
        }

        private static Guid[] GetBigContTypes()
        {
            return GetContTypes("Big");
        }

        private static Guid[] GetContTypes(string sizeName)
        {
            var t = typeof(CalculateRateContainerTypeEnum);
            return Enum.GetNames(t)
                .Where(x => x.StartsWith(sizeName))
                .Select(x => Guid.Parse(((CalculateRateContainerTypeEnum)Enum.Parse(t, x)).GetDescription()))
                .ToArray();
        }

        public string GetTransPicString(ITransPicOrder order, DislocationStation disloc)
        {
            int pointToflag = -1;
            int pointFromflag = -1;

            bool service283Exists = false;
            bool service131FromExists = false;
            bool service131ToExists = false;
            bool serviceShippingExists = false;
            List<Guid> service283PointToCodes = new List<Guid>();
            List<Guid> service283PointFromCodes = new List<Guid>();

            Guid? serviceShippingPointToCode = null;
            Guid? serviceShippingPointFromCode = null;

            Guid? service131FromPointToCode = null;
            Guid? service131FromPointFromCode = null;

            Guid? service131ToPointToCode = null;
            Guid? service131ToPointFromCode = null;

            var transPicParts = new List<TransPicPart>();
            int orderCounter = 0;
            string rapic = GetRaPic(order);

            if (order.Routes != null)
            {
                foreach (var route in order.Routes)
                {
                    if (route.Services.Any(x => x.ParamZoneFromGuid.HasValue))
                    {
                        pointFromflag = 2;
                    }

                    if (route.Services.Any(x => x.ParamZoneToGuid.HasValue))
                    {
                        pointToflag = 2;
                    }

                    if (route.Services.Any(x => x.RefServiceTypeGuid.ToString() == ServicesEnum.Service283.GetDescription()) &&
                        route.FromPointGUID.HasValue && route.ToPointGUID.HasValue)
                    {
                        service283Exists = true;
                        service283PointToCodes.Add(route.FromPointGUID.Value);
                        service283PointToCodes.Add(route.ToPointGUID.Value);
                    }

                    if (route.Services.Any(x => x.RefServiceTypeGuid.ToString() == ServicesEnum.ServiceShipping.GetDescription()))
                    {
                        serviceShippingExists = true;
                        serviceShippingPointFromCode = route.FromPointGUID;
                        serviceShippingPointToCode = route.ToPointGUID;
                    }

                    if (route.Services.Any(x => x.RefServiceTypeGuid.ToString() == ServicesEnum.ServiceShipping.GetDescription()))
                    {
                        if (order.PlaceFromGuid != Guid.Empty && route.FromPointGUID.HasValue && order.PlaceFromGuid == route.FromPointGUID)
                        {
                            service131FromExists = true;
                            service131FromPointFromCode = route.FromPointGUID;
                            service131FromPointToCode = route.ToPointGUID;
                        }

                        if (order.PlaceToGuid != Guid.Empty && route.ToPointGUID.HasValue && order.PlaceToGuid == route.ToPointGUID)
                        {
                            service131ToExists = true;
                            service131ToPointFromCode = route.FromPointGUID;
                            service131ToPointToCode = route.ToPointGUID;
                        }
                    }
                }
            }

            if (pointFromflag < 0 && order.PlaceFromGuid != Guid.Empty)
            {
                pointFromflag = GetPointFlag(order.PlaceFromPointType);
            }

            if (pointToflag < 0 && order.PlaceToGuid != Guid.Empty)
            {
                pointToflag = GetPointFlag(order.PlaceToPointType);
            }

            string fromStr = GetFromPart(pointFromflag);
            orderCounter = AddItem(transPicParts, fromStr, orderCounter, order.PlaceFromGuid != Guid.Empty ? order.PlaceFromGuid : (Guid?)null);

            // Если есть дополнительное ЖД плечо по отправлению:
            if (service131FromExists)
            {
                orderCounter = AddItem(transPicParts, rapic, orderCounter, service131FromPointFromCode, service131FromPointToCode);
            }

            // Если есть фрахт по отправлению:
            if (service283Exists && serviceShippingExists && serviceShippingPointFromCode.HasValue &&
                service283PointToCodes.Any(code => code == serviceShippingPointFromCode.Value))
            {
                orderCounter = AddItem(transPicParts, "fre,$color$/", orderCounter, serviceShippingPointFromCode);
            }

            // Обязательное ЖД плечо:
            int raMainPartOrder = orderCounter;
            orderCounter = AddItem(transPicParts, rapic, orderCounter);

            // Если есть фрахт по назначению:
            if (service283Exists && serviceShippingExists &&
                serviceShippingPointToCode.HasValue &&
                service283PointFromCodes.Any(code => code == serviceShippingPointToCode.Value))
            {
                orderCounter = AddItem(transPicParts, "fre,$color$/", orderCounter, null, serviceShippingPointToCode);
            }

            // Если есть дополнительное ЖД плечо по назначению без слэша в конце строки:
            if (service131ToExists)
            {
                orderCounter = AddItem(transPicParts, rapic, orderCounter, service131ToPointFromCode, service131ToPointToCode);
            }

            string toStr = GetToPart(pointToflag);
            AddItem(transPicParts, toStr, orderCounter,  null, order.PlaceToGuid != Guid.Empty ? order.PlaceToGuid : (Guid?)null);

            SetColors(transPicParts, raMainPartOrder, order, disloc);
            return string.Join(string.Empty, transPicParts.OrderBy(x => x.Order).Select(x => x.ToString()));
        }

        private static string GetToPart(int pointToflag)
        {
            // Если есть автодоставка по отправлению без слэша в конце строки:
            string toPart;
            if (pointToflag == 2)
            {
                toPart = "aut,$color$/wrh,$color$";
            }
            else if (pointToflag == 1)
            {
                toPart = "wrh,$color$";
            }
            else
            {
                toPart = "sta,$color$";
            }

            return toPart;
        }

        private static string GetFromPart(int pointFromflag)
        {
            string fromPart;

            // Если есть автодоставка по отправлению
            if (pointFromflag == 2)
            {
                fromPart = "wrh,$color$/aut,$color$/";
            }
            else if (pointFromflag == 1)
            {
                fromPart = "wrh,$color$/";
            }
            else
            {
                fromPart = "sta,$color$/";
            }

            return fromPart;
        }

        private static string GetRaPic(ITransPicOrder order)
        {
            string rapic = string.Empty;

            if (order.TrainTypeGuid != Guid.Empty)
            {
                if (_containerSmall.Contains(order.TrainTypeGuid))
                {
                    rapic = "ra2,$color$/";
                }

                if (_containerBig.Contains(order.TrainTypeGuid))
                {
                    rapic = "ra4,$color$/";
                }
            }

            return rapic;
        }

        private static int GetPointFlag(PointTypeEnum placePointType)
        {
            return (byte)(_pointStationTypeCondition.Contains(placePointType) ? 0 : (placePointType == PointTypeEnum.PointPortType ? 1 : -1));
        }

        private static int AddItem(List<TransPicPart> list, string part, int order, Guid? from = null, Guid? to = null)
        {
            list.Add(new TransPicPart()
            {
                Order = order,
                Part = part,
                FromGuid = from,
                ToGuid = to
            });
            return ++order;
        }

        private static void SetColors(List<TransPicPart> list, int raMainPartOrder, ITransPicOrder order, DislocationStation disloc)
        {
            // Если не ТЕО документ, то не выставляем цвета
            if (!order.IsTeo)
            {
                return;
            }

            // Если не определена станция дислокации, то тоже не выставляем цвета
            if (disloc == null)
            {
                return;
            }

            // В начальном пункте
            if (order.PlaceFromGuid == disloc.ReferenceGUID)
            {
                return;
            }

            // в конечном пункте, все звенья зелёные
            if (order.PlaceToGuid == disloc.ReferenceGUID)
            {
                list.ForEach(x => x.PartColor = TransPicPartEnum.Green);
                return;
            }

            var mainPart = list.Single(x => x.Order == raMainPartOrder);
            mainPart.FromGuid = list.Single(x => x.Order == raMainPartOrder - 1).ToGuid;
            mainPart.ToGuid = list.Single(x => x.Order == raMainPartOrder + 1).FromGuid;

            int yellowOrder = 0;
            foreach (var part in list)
            {
                if (part.FromGuid.HasValue && part.FromGuid.Value == disloc.ReferenceGUID)
                {
                    yellowOrder = part.Order;
                    break;
                }

                if (part.ToGuid.HasValue && part.ToGuid.Value == disloc.ReferenceGUID)
                {
                    yellowOrder = part.Order + 1;
                    break;
                }
            }

            list.Where(x => x.Order < yellowOrder).DoForEach(x => x.PartColor = TransPicPartEnum.Green);
            var yellowPart = list.FirstOrDefault(x => x.Order == yellowOrder);

            if (yellowPart != null)
            {
                yellowPart.PartColor = TransPicPartEnum.Yellow;
            }
        }
    }

    internal class TransPicPart
    {
        public int Order { get; set; }

        public string Part { get; set; }

        public TransPicPartEnum PartColor { get; set; }

        public Guid? FromGuid { get; set; }

        public Guid? ToGuid { get; set; }

        public override string ToString()
        {
            return Part.Replace("$color$", PartColor.GetDescription());
        }
    }

    enum TransPicPartEnum
    {
        [Description("b")]
        Blue = 0,

        [Description("y")]
        Yellow = 1,

        [Description("g")]
        Green = 2,
    }
}
