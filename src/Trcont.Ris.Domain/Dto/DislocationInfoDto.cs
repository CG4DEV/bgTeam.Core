namespace Trcont.Ris.Domain.Dto
{
    using System;

    public class DislocationInfoDto
    {
        public string ContNumber { get; set; }

        public DateTime Date { get; set; }

        public StationDto Station { get; set; }

        public ValueDto Operation { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }
    }
}
