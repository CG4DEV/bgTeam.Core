namespace Trcont.Ris.Common.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using bgTeam.Extensions;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.TransPicture;
    /*[Obsolete]
    public class OldTransPicService : ITransPicService
    {
        private static readonly Guid[] _containerSmall = GetSmallContTypes();
        private static readonly Guid[] _containerBig = GetBigContTypes();

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

        public string GetTransPicString(ITransPicOrder order, IEnumerable<FactByRouteDto> facts)
        {
            FactObjectDto fact = null;
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
                    if (route.Services.Any(x => x.IsAutoFromExist))
                    {
                        pointFromflag = 2;
                    }

                    if (route.Services.Any(x => x.IsAutoToExist))
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

                    if (route.Services.Any(x => x.RefServiceTypeGuid.ToString() == ServicesEnum.ShippingService.GetDescription()))
                    {
                        serviceShippingExists = true;
                        serviceShippingPointFromCode = route.FromPointGUID;
                        serviceShippingPointToCode = route.ToPointGUID;
                    }

                    if (route.Services.Any(x => x.RefServiceTypeGuid.ToString() == ServicesEnum.ShippingService.GetDescription()))
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

            var fromPart = GetFromPart(pointFromflag);
            orderCounter = AddItem(transPicParts, fromPart, orderCounter, order.PlaceFromGuid != Guid.Empty ? order.PlaceFromGuid : (Guid?)null);

            // Если есть дополнительное ЖД плечо по отправлению:
            if (service131FromExists)
            {
                var part = new PartInfo()
                {
                    Part = rapic,
                    Type = TransType.Main
                };
                orderCounter = AddItem(transPicParts, part, orderCounter, service131FromPointFromCode, service131FromPointToCode);
            }

            // Если есть фрахт по отправлению:
            if (service283Exists && serviceShippingExists && serviceShippingPointFromCode.HasValue &&
                service283PointToCodes.Any(code => code == serviceShippingPointFromCode.Value))
            {
                var part = new PartInfo()
                {
                    Part = "fre,$color$/",
                    Type = TransType.Main
                };
                orderCounter = AddItem(transPicParts, part, orderCounter, serviceShippingPointFromCode);
            }

            // Обязательное ЖД плечо:
            int raMainPartOrder = orderCounter;
            var mainPart = new PartInfo()
            {
                Part = rapic,
                Type = TransType.Main
            };
            orderCounter = AddItem(transPicParts, mainPart, orderCounter);

            // Если есть фрахт по назначению:
            if (service283Exists && serviceShippingExists &&
                serviceShippingPointToCode.HasValue &&
                service283PointFromCodes.Any(code => code == serviceShippingPointToCode.Value))
            {
                var part = new PartInfo()
                {
                    Part = "fre,$color$/",
                    Type = TransType.Main
                };
                orderCounter = AddItem(transPicParts, part, orderCounter, null, serviceShippingPointToCode);
            }

            // Если есть дополнительное ЖД плечо по назначению без слэша в конце строки:
            if (service131ToExists)
            {
                var part = new PartInfo()
                {
                    Part = rapic,
                    Type = TransType.Main
                };
                orderCounter = AddItem(transPicParts, part, orderCounter, service131ToPointFromCode, service131ToPointToCode);
            }

            var toPart = GetToPart(pointToflag);
            AddItem(transPicParts, toPart, orderCounter, null, order.PlaceToGuid != Guid.Empty ? order.PlaceToGuid : (Guid?)null);

            if (fact != null)
            {
                SetColors(transPicParts, raMainPartOrder, order, fact);
            }

            return string.Join(string.Empty, transPicParts.OrderBy(x => x.Order).Select(x => x.ToString()));
        }

        private static PartInfo GetToPart(int pointToflag)
        {
            // Если есть автодоставка по отправлению без слэша в конце строки:
            var part = new PartInfo();

            if (pointToflag == 2)
            {
                part.Type = TransType.AutTo;
                part.Part = "aut,$color$/wrh,$color$";
            }
            else if (pointToflag == 1)
            {
                part.Type = TransType.StaTo;
                part.Part = "wrh,$color$";
            }
            else
            {
                part.Type = TransType.StaTo;
                part.Part = "sta,$color$";
            }

            return part;
        }

        private static PartInfo GetFromPart(int pointFromflag)
        {
            var part = new PartInfo();

            // Если есть автодоставка по отправлению
            if (pointFromflag == 2)
            {
                part.Type = TransType.AutFrom;
                part.Part = "wrh,$color$/aut,$color$/";
            }
            else if (pointFromflag == 1)
            {
                part.Type = TransType.StaFrom;
                part.Part = "wrh,$color$/";
            }
            else
            {
                part.Type = TransType.StaFrom;
                part.Part = "sta,$color$/";
            }

            return part;
        }

        private static string GetRaPic(ITransPicOrder order)
        {
            string rapic = null;
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
            return (byte)(placePointType == PointTypeEnum.PointStationType ? 0 : (placePointType == PointTypeEnum.PointPortType ? 1 : -1));
        }

        private static int AddItem(List<TransPicPart> list, PartInfo part, int order, Guid? from = null, Guid? to = null)
        {
            list.Add(new TransPicPart()
            {
                Order = order,
                Part = part.Part,
                FromGuid = from,
                ToGuid = to,
                 Type = part.Type
            });
            return ++order;
        }

        private static void SetColors(List<TransPicPart> list, int raMainPartOrder, ITransPicOrder order, FactObjectDto fact)
        {
            // Если не ТЕО документ, то не выставляем цвета
            if (!order.TeoId.HasValue)
            {
                return;
            }

            if (fact.AutoFromDone)
            {
                list.Where(x => x.Type == TransType.AutFrom).DoForEach(x => x.PartColor = TransPicPartEnum.Green);
            }

            if (fact.AutoToDone)
            {
                list.Where(x => x.Type == TransType.AutTo).DoForEach(x => x.PartColor = TransPicPartEnum.Green);
            }

            if (fact.MainStarted != MainState.NotStarted)
            {
                list.Where(x => x.Type == TransType.StaFrom).DoForEach(x => x.PartColor = fact.MainStarted == MainState.InTransit ? TransPicPartEnum.Yellow : TransPicPartEnum.Green);
            }

            if (fact.MainDone != MainState.NotStarted)
            {
                list.Where(x => x.Type == TransType.StaTo).DoForEach(x => x.PartColor = fact.MainDone == MainState.InTransit ? TransPicPartEnum.Yellow : TransPicPartEnum.Green);
            }

            if (fact.MainStarted != MainState.NotStarted)
            {
                if (fact.MainDone == MainState.InTransit)
                {
                    list.Where(x => x.Type == TransType.StaTo).DoForEach(x => x.PartColor = TransPicPartEnum.Yellow);
                }

                if (fact.MainDone == MainState.Arrive)
                {
                    list.Where(x => x.Type == TransType.StaTo).DoForEach(x => x.PartColor = TransPicPartEnum.Green);
                }
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

        public TransType Type { get; set; }

        public override string ToString()
        {
            return Part.Replace("$color$", PartColor.GetDescription());
        }
    }

    internal class PartInfo
    {
        public string Part { get; set; }

        public TransType Type { get; set; }
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

    enum TransType
    {
        Unknown = 0,
        AutFrom = 1,
        StaFrom = 2,
        StaTo = 3,
        AutTo = 4,
        Main = 5,
        //FreFrom = 6,
        //FreTo = 7,
        //RaAddFrom = 8,
        //RaAddTo = 9,
    }*/
}
