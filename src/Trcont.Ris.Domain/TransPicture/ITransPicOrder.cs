namespace Trcont.Ris.Domain.TransPicture
{
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Dto;
    using Trcont.Ris.Domain.Enums;

    public interface ITransPicOrder
    {
        //Guid ReferenceGuid { get; set; }

        OrderStatusEnum StatusId { get; set; }

        //bool IsTeo { get; set; }

        int? TeoId { get; set; }

        Guid TrainTypeGuid { get; set; }

        Guid PlaceFromGuid { get; set; }

        Guid PlaceToGuid { get; set; }

        PointTypeEnum PlaceFromPointType { get; set; }

        PointTypeEnum PlaceToPointType { get; set; }

        string TransPic { get; set; }

        IEnumerable<OrderRouteDto> Routes { get; set; }
    }

    public interface IDislocationStation
    {
        Guid ReferenceGUID { get; set; }

        string StationTitle { get; set; }

        string StationCode { get; set; }

        bool IsPort { get; set; }
    }
}
