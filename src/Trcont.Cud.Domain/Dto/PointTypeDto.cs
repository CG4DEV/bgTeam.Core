namespace Trcont.Cud.Domain.Dto
{
    using System;
    using Trcont.Cud.Domain.Enum;

    public class PointTypeDto
    {
        public Guid PlaceGuid { get; set; }

        public PointTypeEnum PointType { get; set; }
    }
}
