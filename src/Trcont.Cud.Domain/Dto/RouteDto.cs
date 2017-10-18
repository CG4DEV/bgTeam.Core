namespace Trcont.Cud.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RouteDto
    {
        public RouteDto(Guid? from, Guid? to)
        {
            From = from;
            To = to;
        }

        public Guid? From { get; private set; }

        public Guid? To { get; private set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var routeY = (RouteDto)obj;
            return routeY.From == this.From &&
                   routeY.To == this.To;
        }

        public override int GetHashCode()
        {
            int result = 0;
            if (From.HasValue)
            {
                result = From.Value.GetHashCode() * 17;
            }

            if (To.HasValue)
            {
                result += To.Value.GetHashCode() * 17;
            }

            return result;
        }
    }
}
