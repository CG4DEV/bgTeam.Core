namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;

    public class PointServices
    {
        public string InputBeginPointCnsi { get; set; }

        public Guid? BeginPointGuid { get; set; }

        public string BeginPointCnsi { get; set; }

        public string BeginPointTitle { get; set; }


        public string InputEndPointCnsi { get; set; }

        public Guid? EndPointGuid { get; set; }

        public string EndPointCnsi { get; set; }

        public string EndPointTitle { get; set; }

        public IEnumerable<MinServiceParamDto> ServiceParams { get; set; }
    }
}
