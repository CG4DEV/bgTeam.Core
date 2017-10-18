namespace Trcont.Ris.Domain.Dto
{
    using Trcont.Ris.Domain.TransPicture;

    public class RouteServiceOrderServiceDto
    {
        public int ServiceId { get; set; }

        public int OrderId { get; set; }

        public int TeoId { get; set; }

        public int ServiceTypeId { get; set; }

        public int? FromPointId { get; set; }

        public string FromPointCode { get; set; }

        public string FromPointTitle { get; set; }

        public PointTypeEnum FromPointType { get; set; }

        public int? ToPointId { get; set; }

        public string ToPointCode { get; set; }

        public string ToPointTitle { get; set; }

        public PointTypeEnum ToPointType { get; set; }

        public int ArmIndex { get; set; }
    }
}
