namespace Trcont.Ris.Story.Common
{
    using System;
    using System.Collections.Generic;

    public class GetParamsValueByServiceIdStoryContext
    {
        public IEnumerable<ContextForParamValues> InputParams { get; set; }
    }

    public class ContextForParamValues
    {
        public IEnumerable<string> ServiceCodes { get; set; }

        public IEnumerable<int?> ServiceIds { get; set; }


        public string BeginPointCnsi { get; set; }

        public Guid? OutBeginPointGuid { get; set; }

        public string OutBeginPointTitle { get; set; }

        public string OutBeginPointCnsi { get; set; }


        public string EndPointCnsi { get; set; }

        public Guid? OutEndPointGuid { get; set; }

        public string OutEndPointTitle { get; set; }

        public string OutEndPointCnsi { get; set; }
    }
}