namespace Trcont.Ris.Domain.Dto
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.TransPicture;

    public class OrderInfoDto : Order
    {
        public string StatusTitle { get; set; }

        public Guid ClientGuid { get; set; }

        public Guid ContractGuid { get; set; }

        public Guid TrainTypeGuid { get; set; }

        public string TrainTypeTitle { get; set; }

        public string TrainTypeCNSICode { get; set; }

        public Guid? TrainTypeCNSIGuid { get; set; }

        public string ETSNGTitle { get; set; }

        public string ETSNGCode { get; set; }

        public string GNGTitle { get; set; }

        public string GNGCode { get; set; }

        public string CurrencyTitle { get; set; }

        public Guid PlaceFromGuid { get; set; }

        public string PlaceFromTitle { get; set; }

        public string PlaceFromCnsiCode { get; set; }

        public Guid? PlaceFromCnsiGuid { get; set; }

        public string CountryFromTitle { get; set; }

        public Guid PlaceToGuid { get; set; }

        public string PlaceToTitle { get; set; }

        public string PlaceToCnsiCode { get; set; }

        public Guid? PlaceToCnsiGuid { get; set; }

        public string CountryToTitle { get; set; }

        public decimal Summ { get; set; }

        public string ContrRelationsTypeTitle { get; set; }

        public string SendTypeTitle { get; set; }

        public string TransTypeTitle { get; set; }

        //Из факта

        public DateTime? ArrivalDate { get; set; }

        public DateTime? PlanArrivalDate { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? LoadDate { get; set; }

        //

        public PointTypeEnum PlaceFromPointType { get; set; }

        public PointTypeEnum PlaceToPointType { get; set; }

        public IEnumerable<OrderRouteDto> Routes { get; set; }
    }
}
