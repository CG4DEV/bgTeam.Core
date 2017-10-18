namespace Trcont.Cud.Domain
{
    using System;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Enum;

    public interface ITransPicOrder
    {
        Guid ReferenceGuid { get; set; }

        OrderStatusEnum Status { get; set; }

        bool IsTeo { get; set; }

        Guid TrainTypeGuid { get; set; }

        Guid PlaceFromGuid { get; set; }

        Guid PlaceToGuid { get; set; }

        PointTypeEnum PlaceFromPointType { get; set; }

        PointTypeEnum PlaceToPointType { get; set; }

        string TransPic { get; set; }

        IEnumerable<IRoute> Routes { get; set; }
    }
}
