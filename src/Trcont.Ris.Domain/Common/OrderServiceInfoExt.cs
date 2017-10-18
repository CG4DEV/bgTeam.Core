namespace Trcont.Ris.Domain.Common
{
    using System;
    using Trcont.App.Service.Entity;
    using Trcont.Domain.Common;

    public class OrderServiceInfoExt : OrderServiceInfo, IRouteServiceInfo
    {
        public string FromPointCode { get; set; }

        public string ToPointCode { get; set; }

        public int FromPointId { get; set; }

        public int ToPointId { get; set; }


        //public int ServiceTypeId { get; set; }

        public int ServiceId { get; set; }

        public string ServiceCode { get; set; }

        public string ServiceTitle { get; set; }

        public int ServiceFortId { get; set; }


        public ServiceTypeEnum ServiceType { get; set; }


        public RouteForSave Route { get; set; }

        public OrderServiceAttribute[] Attributes2 { get; set; }

        public bool IsActive { get; set; }

        public int IsRequired { get; set; }

        public Guid? TariffGuid { get; set; }
    }
}
