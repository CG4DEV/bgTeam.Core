namespace Trcont.Cud.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Enum;

    public class OrderMinDto : ITransPicOrder
    {
        public Guid ReferenceGuid { get; set; }

        public string Number { get; set; }

        public OrderStatusEnum Status { get; set; }

        public bool IsTeo { get; set; }

        public Guid TrainTypeGuid { get; set; }

        public Guid PlaceFromGuid { get; set; }

        public Guid PlaceToGuid { get; set; }

        public PointTypeEnum PlaceFromPointType { get; set; }

        public PointTypeEnum PlaceToPointType { get; set; }

        public string TransPic { get; set; }

        public IEnumerable<IRoute> Routes { get; set; }
    }
}
