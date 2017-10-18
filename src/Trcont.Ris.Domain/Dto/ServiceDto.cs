namespace Trcont.Ris.Domain.Dto
{
    using System.Collections.Generic;

    public class ServiceDto
    {
        public string ServiceCode { get; set; }

        public int? ServiceId { get; set; }

        public IEnumerable<ServiceParamDto> Params { get; set; }
    }
}
