namespace Trcont.Ris.Domain.Entity
{
    using DapperExtensions.Attributes;
    using Trcont.Ris.Domain.Enums;

    [TableName("OrdersRoutes")]
    public class OrdersRoute : EntityBaseIdentity
    {
        public int OrderId { get; set; }

        public int TeoId { get; set; }

        public int? FromPointId { get; set; }

        public string FromPointCode { get; set; }

        public string FromPointTitle { get; set; }

        public int? ToPointId { get; set; }

        public string ToPointCode { get; set; }

        public string ToPointTitle { get; set; }

        public int? ArmIndex { get; set; }

        public RouteTypeEnum RouteType { get; set; }
}
}
