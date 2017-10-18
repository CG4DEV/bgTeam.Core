namespace Trcont.OTM.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Trcont.Domain.OtmData;
    using Trcont.OTM.Service.WebService;

    internal static class ReleaseHelper
    {
        private const string TRCONT = "TRCONT";
        private const string ROUTE_SEPARATOR = "-";
        private const string DATE_FORMAT = "yyyyMMddhhmmss";


        public static Release GetRelease(OtmRelease otmRelease)
        {
            Release release = new Release();

            release.TotalPackagedItemCount = otmRelease.CargoList.Count().ToString();
            release.ReleaseGid = GetReleaseGid(otmRelease);
            release.TransactionCode = "RC";
            release.ReleaseHeader = GetReleaseHeader(otmRelease);
            release.ReleaseStatus = new[] { GetReleaseStatus(otmRelease) };
            release.TimeWindow = GetTimeWindow(otmRelease);
            release.ShipFromLocationRef = new ShipFromLocationRef() { LocationRef = GetLocationRef(otmRelease.StationFrom) };
            release.ShipToLocationRef = new ShipToLocationRef() { LocationRef = GetLocationRef(otmRelease.StationTo) };
            release.ReleaseLine = GetReleaseLines(otmRelease);
            release.ShipUnit = GetShipUnits(otmRelease);
            release.ReleaseTypeGid = new ReleaseTypeGid()
            {
                Gid = new Gid() { Xid = "CUSTOMER_ORDER" }
            };

            return release;
        }

        public static ShipUnit[] GetShipUnits(OtmRelease otmRelease)
        {
            List<ShipUnit> shipUnits = new List<ShipUnit>();
            int i = 1;

            foreach (var item in otmRelease.CargoList)
            {
                ShipUnit shipUnit = new ShipUnit();
                Volume volume = new Volume()
                {
                    VolumeValue = item.Volume.ToString().Replace(',', '.'),
                    VolumeUOMGid = new VolumeUOMGid() { Gid = new Gid() { Xid = item.VolumeUom } }
                };

                shipUnit.ShipUnitGid = new ShipUnitGid() { Gid = GetGid($"{otmRelease.OrderId}-{i}") };
                shipUnit.TransactionCode = "IU";
                shipUnit.WeightVolume = new WeightVolume()
                {
                    Weight = new Weight()
                    {
                        WeightValue = item.WeightBrutto.ToString().Replace(',', '.'),
                        WeightUOMGid = new WeightUOMGid() { Gid = new Gid() { Xid = item.WeightUom } }
                    },
                    Volume = volume
                };
                shipUnit.UnitNetWeightVolume = new UnitNetWeightVolume()
                {
                    Weight = new Weight()
                    {
                        WeightValue = item.WeightNet.ToString().Replace(',', '.'),
                        WeightUOMGid = new WeightUOMGid() { Gid = new Gid() { Xid = item.WeightUom } }
                    },
                    Volume = volume
                };
                shipUnit.ShipUnitCount = "1";
                shipUnit.ShipUnitContent = new[]
                {
                    new ShipUnitContent()
                    {
                        ReleaseGid = new ReleaseGid() { Gid = GetGid(otmRelease.OrderId) },
                        ReleaseLineGid = new ReleaseLineGid() { Gid = GetGid($"{otmRelease.OrderId}-{i}") },
                        PackagedItemRef = GetPackagedItemRef(item),
                        LineNumber = "1",
                        WeightVolumePerShipUnit = new WeightVolumePerShipUnit()
                        {
                            Weight = new Weight()
                            {
                                WeightValue = item.WeightNet.ToString().Replace(',', '.'),
                                WeightUOMGid = new WeightUOMGid() { Gid = new Gid() { Xid = item.WeightUom } }
                            }
                        }
                    }
                };

                shipUnits.Add(shipUnit);
                i++;
            }

            return shipUnits.ToArray();
        }

        public static ReleaseLine[] GetReleaseLines(OtmRelease otmRelease)
        {
            List<ReleaseLine> releaseLines = new List<ReleaseLine>();
            int i = 1;

            foreach (var item in otmRelease.CargoList)
            {
                ReleaseLine releaseLine = new ReleaseLine();

                releaseLine.ReleaseLineGid = new ReleaseLineGid() { Gid = GetGid($"{otmRelease.OrderId}-{i}") };
                releaseLine.PackagedItemRef = GetPackagedItemRef(item);
                releaseLine.InitialItemGid = new InitialItemGid() { Gid = GetGid($"{item.EstngCode.PadRight(6, '0')}-00000000") };
                releaseLine.TransportHandlingUnitRef = new TransportHandlingUnitRef()
                {
                    ShipUnitSpecRef = new ShipUnitSpecRef()
                    {
                        Item = new ShipUnitSpecGid() { Gid = GetGid(item.ContainerType) }
                    }
                };

                releaseLines.Add(releaseLine);
                i++;
            }

            return releaseLines.ToArray();
        }

        public static PackagedItemRef GetPackagedItemRef(OtmCargo otmCargo)
        {
            PackagedItemRef packagedItemRef = new PackagedItemRef();

            packagedItemRef.Item = GetPackagedItem(otmCargo);

            return packagedItemRef;
        }

        public static PackagedItem GetPackagedItem(OtmCargo otmCargo)
        {
            PackagedItem packagedItem = new PackagedItem();

            packagedItem.Packaging = new Packaging()
            {
                PackagedItemGid = new PackagedItemGid() { Gid = GetGid($"{otmCargo.EstngCode.PadRight(6, '0')}-00000000") }
            };
            packagedItem.Item = new Item()
            {
                TransactionCode = "NP",
                ItemGid = new ItemGid() { Gid = GetGid($"{otmCargo.EstngCode.PadRight(6, '0')}-00000000") }
            };

            return packagedItem;
        }

        public static LocationRef GetLocationRef(string station)
        {
            LocationRef locationRef = new LocationRef();

            locationRef.Item = new LocationGid()
            {
                Gid = GetGid(station)
            };

            return locationRef;
        }

        public static TimeWindow GetTimeWindow(OtmRelease otmRelease)
        {
            TimeWindow timeWindow = new TimeWindow();

            timeWindow.EarlyPickupDt = new GLogDateTimeType()
            {
                GLogDate = otmRelease.DateLoad.ToString(DATE_FORMAT)
            };
            timeWindow.EarlyDeliveryDt = new GLogDateTimeType()
            {
                GLogDate = otmRelease.DateArrival.ToString(DATE_FORMAT)
            };

            return timeWindow;
        }

        public static ReleaseStatus GetReleaseStatus(OtmRelease otmRelease)
        {
            ReleaseStatus releaseStatus = new ReleaseStatus();

            releaseStatus.StatusTypeGid = new StatusTypeGid() { Gid = GetGid(otmRelease.StatusType ?? "ИСПОЛНЕНИЕ_ЗАКАЗА") };
            releaseStatus.StatusValueGid = new StatusValueGid() { Gid = GetGid(otmRelease.StatusName ?? "НА_ПЛАНИРОВАНИЕ") };

            return releaseStatus;
        }

        public static ReleaseHeader GetReleaseHeader(OtmRelease otmRelease)
        {
            ReleaseHeader releaseHeader = new ReleaseHeader();

            releaseHeader.ReleaseName = otmRelease.OrderName;
            releaseHeader.ReleaseMethodGid = new ReleaseMethodGid() { Gid = GetGid("XX_THU-CONT", "PUBLIC") };
            releaseHeader.CommercialTerms = new CommercialTerms()
            {
                IncoTermGid = new IncoTermGid() { Gid = GetGid("ISALES") }
            };

            var gid = GetGid(string.Join(ROUTE_SEPARATOR, otmRelease.Routes));
            releaseHeader.FixedItineraryGid = new FixedItineraryGid(){ Gid = gid };
            releaseHeader.FixedSellItineraryGid = new FixedSellItineraryGid() { Gid = gid };
            releaseHeader.TimeWindowEmphasisGid = new TimeWindowEmphasisGid() { Gid = GetGid("PAST", string.Empty) };

            return releaseHeader;
        }

        public static ReleaseGid GetReleaseGid(OtmRelease otmRelease)
        {
            ReleaseGid releaseGid = new ReleaseGid();
            releaseGid.Gid = GetGid(otmRelease.OrderId);
            return releaseGid;
        }

        public static Gid GetGid(string value, string domain = null)
        {
            return new Gid()
            {
                DomainName = domain ?? TRCONT,
                Xid = value
            };
        }
    }
}
