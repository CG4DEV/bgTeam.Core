namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Enums;
    using Trcont.Ris.Domain.TransPicture;

    public class OrderMinDto : ITransPicOrder
    {
        public int Id { get; set; }

        public int? TeoId { get; set; }

        public string Number { get; set; }

        public OrderStatusEnum StatusId { get; set; }

        public Guid TrainTypeGuid { get; set; }

        public int PlaceFromId { get; set; }

        public Guid PlaceFromGuid { get; set; }

        public int PlaceToId { get; set; }

        public Guid PlaceToGuid { get; set; }

        public int? ContainerQuantity { get; set; }

        public PointTypeEnum PlaceFromPointType { get; set; }

        public PointTypeEnum PlaceToPointType { get; set; }

        public string TransPic { get; set; }

        public IEnumerable<OrderRouteDto> Routes { get; set; }
    }
}
