namespace Trcont.Ris.Domain.Common
{
    using System;

    public class RouteDto : IBaseRoute
    {
        public RouteDto(Guid? from, Guid? to, int armIndex)
        {
            FromPointGUID = from;
            ToPointGUID = to;
            ArmIndex = armIndex;
        }

        public Guid? FromPointGUID { get; set; }

        public Guid? ToPointGUID { get; set; }

        public int ArmIndex { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var routeY = (RouteDto)obj;
            return routeY.FromPointGUID == this.FromPointGUID &&
                   routeY.ToPointGUID == this.ToPointGUID;
        }

        public override int GetHashCode()
        {
            int result = 0;
            if (FromPointGUID.HasValue)
            {
                result = FromPointGUID.Value.GetHashCode() * 17;
            }

            if (ToPointGUID.HasValue)
            {
                result += ToPointGUID.Value.GetHashCode() * 17;
            }

            return result;
        }
    }
}
