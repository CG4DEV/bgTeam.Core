namespace Trcont.Ris.DataAccess.Services
{
    using System;
    using System.Collections.Generic;

    public class GetParamsForRouteCmdContext
    {
        public int[] ServiceIds { get; set; }
        public Guid[] IgnoreParams { get; set; }
    }
}