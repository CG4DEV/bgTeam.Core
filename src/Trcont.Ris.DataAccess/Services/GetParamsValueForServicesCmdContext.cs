namespace Trcont.Ris.DataAccess.Services
{
    using System;

    public class GetParamsValueForServicesCmdContext
    {
        public int? ServiceId { get; set; }

        public Guid? PointIrsGuid { get; set; }
    }
}