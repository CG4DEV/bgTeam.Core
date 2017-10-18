namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class RouteServiceOrderDto
    {
        public int Id { get; set; }

        public Guid IrsGuid { get; set; }

        public int? TeoId { get; set; }

        public Guid? TeoGuid { get; set; }

        public int? PlaceFromId { get; set; }

        public int? PlaceToId { get; set; }

        public IEnumerable<RouteServiceOrderServiceDto> Services { get; set; }
    }
}
