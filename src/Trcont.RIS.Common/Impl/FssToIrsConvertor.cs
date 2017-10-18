namespace Trcont.Ris.Common.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.Extensions;
    using Trcont.Domain.Common;
    using Trcont.Domain.FssData;
    using Trcont.Ris.Common.Helpers;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Entity;

    public class FssToIrsConvertor : IFssToIrsConvertor
    {
        private readonly IAppLogger _logger;
        private readonly IRepositoryEntity _repository;

        private readonly Hashtable _currencyList;

        public FssToIrsConvertor(
            IAppLogger logger, 
            IRepositoryEntity repository)
        {
            _logger = logger;
            _repository = repository;

            _currencyList = new Hashtable();

            // Валюта
            _currencyList.Add(643, 1);
            _currencyList.Add(840, 2);
            _currencyList.Add(978, 71698);
        }

        public string Convert(string xml, string routeId, string routeSpecId, IRouteServiceInfo[] services)
        {
            return ConvertAsync(xml, routeId, routeSpecId, services).Result;
        }

        public async Task<string> ConvertAsync(string xml, string routeId, string routeSpecId, IRouteServiceInfo[] services)
        {
            var result = XmlHelper.DeserializeXmlFromString<FssXmlRoot>(xml);

            if(routeSpecId == string.Empty)
            {
                routeSpecId = null;
            }

            //var itinerary = result.DeliveryRouteList.SingleOrDefault(x => x.RouteId == routeId);
            var itinerary = result.DeliveryRouteList
                .Where(x => x.RouteId == routeId)
                .SingleOrDefault(x => x.SpecialRateUId == routeSpecId);

            if (itinerary == null)
            {
                throw new ArgumentException("Not find route");
            }

            //нужно выбирать спец ставку


            var droute = new IrsXmlDeliveryRoute();

            droute.RouteId = 1;
            droute.Priority = 1;
            droute.Label = itinerary.Label;
            droute.Duration = itinerary.Duration;

            foreach (var item in services)
            {
                var route = itinerary.DeliveryArmList.SingleOrDefault(x => x.FromPointCode == item.FromPointCode && x.ToPointCode == item.ToPointCode);
                if (route == null)
                {
                    continue;
                }

                var darm = new IrsXmlDeliveryArm();

                darm.LevelUpServiceTypeID = item.ServiceId;
                darm.FromPointId = item.FromPointId;
                darm.ToPointId = item.ToPointId;
                darm.Label = item.ServiceTitle;
                darm.Duration = route.Duration;
                darm.Lenght = route.Lenght;

                // Поиск через маппинг
                //var internalSrv = await _repository.GetAllAsync<ServiceInternalInfo>(
                //    @"SELECT r.Id as ServiceId, r.Code as ServiceCode FROM Srv2SrvMap ssm
                //        INNER JOIN Service r ON r.Id = ssm.InternalSrvId
                //        WHERE ssm.ExternalSrvId = @ExternalSrvId AND r.ServiceGroupId = @ServiceGroup", new { ExternalSrvId = item.ServiceId, ServiceGroup = 15359 /*ЕСУ*/ });

                //var routeService = route.GetAllService();
                //foreach (var srv in routeService)
                //{
                //    var res = internalSrv.SingleOrDefault(x => x.ServiceCode == srv.ServiceCode);
                //    if (res != null)
                //    {
                //        darm.ServiceMandatory.Add(
                //            new IrsXmlServiceMandatory()
                //            {
                //                ServiceTypeId = res.ServiceId,
                //                Rate = srv.Cost,
                //                ExchangeRate = 1,
                //                CurrencyId = (int)_currencyList[srv.Currency],
                //                TariffType = srv.TariffType,
                //                StdVolume = srv.StdVolume,
                //                ContractId = srv.ContractRegNum == "120-жд"? "239509" : null,
                //                ActualPeriodStart = srv.ActualPeriodStart.HasValue ? srv.ActualPeriodStart.ToString() : null,
                //                ActualPeriodEnd = srv.ActualPeriodEnd.HasValue ? srv.ActualPeriodEnd.ToString() : null,
                //                TerritoryId = srv.Territory == 643 ? "19271" : null // указываем только россию
                //            });
                //    }
                //}

                IEnumerable<FssXmlServiceBase> routeService;
                switch (item.ServiceType)
                {

                    //case ServiceTypeEnum.Mandatory:
                    //    routeService = route.ServiceMandatoryList.Where(x => x.ParentUSLCode == item.ServiceCode && x.Cost >= 0);
                    //    break;
                    //case ServiceTypeEnum.Recomended:
                    //    routeService = route.ServiceRecommendedList.Where(x => x.ParentUSLCode == item.ServiceCode && x.Cost >= 0);
                    //    break;
                    //case ServiceTypeEnum.Extra:
                    //    routeService = route.ExtraServiceList.Where(x => x.ParentUSLCode == item.ServiceCode && x.Cost >= 0);
                    //    break;

                    case ServiceTypeEnum.Mandatory:
                        routeService = route.ServiceMandatoryList.Where(x => x.ServiceId == item.ServiceFortId);
                        break;
                    case ServiceTypeEnum.Recomended:
                        routeService = route.ServiceRecommendedList.Where(x => x.ServiceId == item.ServiceFortId);
                        break;
                    case ServiceTypeEnum.Extra:
                        routeService = route.ExtraServiceList.Where(x => x.ServiceId == item.ServiceFortId);
                        break;

                    default:
                        throw new ArgumentException("ServiceType not find");
                }

                //var routeService = route
                //    .GetAllService()
                //    .Where(x => x.ParentUSLCode == item.ServiceCode && x.Cost >= 0);

                foreach (var srv in routeService)
                {
                    var srvs = await _repository.GetAsync<Service>(x => x.Code == srv.USLCode && x.ServiceGroupId == (int)ServiceGroupEnum.ESU);

                    darm.ServiceMandatory.Add(
                        new IrsXmlServiceMandatory()
                        {
                            ServiceTypeId = srvs.Id.Value,
                            Rate = srv.Cost,
                            ExchangeRate = 1,
                            CurrencyId = (int)_currencyList[srv.Currency],
                            TariffType = srv.TariffType,
                            StdVolume = srv.StdVolume,
                            ContractId = srv.ContractRegNum == "120-жд" ? "239509" : null,
                            ActualPeriodStart = srv.ActualPeriodStart,
                            ActualPeriodEnd = srv.ActualPeriodEnd,
                            TerritoryId = srv.Territory == 643 ? "19271" : null, // указываем только россию
                            Label = srv.USLCode,
                        });
                }

                droute.DeliveryArm.Add(darm);
            }

            var main = new IrsXmlMainRoot(droute);

            var str = XmlHelper.SerializeToString(main)
                .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", null);
                //.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", null);

#if DEBUG
            //File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}/temp/{routeId}_fss.xml", xml);
            //File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}/temp/{routeId}_irs.xml", str);
#endif
            return str;
        }

        //class ServiceInternalInfo : IServiceInfo
        //{
        //    public int ServiceId { get; set; }

        //    public string ServiceCode { get; set; }

        //    public string ServiceTitle { get; set; }
        //    public ServiceTypeEnum ServiceType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        //}

        //public class ServiceCodeComparer : IEqualityComparer<IServiceInfo>
        //{
        //    public int GetHashCode(IServiceInfo co)
        //    {
        //        if (co == null)
        //        {
        //            return 0;
        //        }

        //        return co.ServiceCode.GetHashCode();
        //    }

        //    public bool Equals(IServiceInfo x1, IServiceInfo x2)
        //    {
        //        if (x1.ServiceCode == x2.ServiceCode)
        //        {
        //            return true;
        //        }

        //        return false;
        //    }
        //}
    }
}
