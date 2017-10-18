namespace Trcont.Ris.DataAccess.Services
{
    using Trcont.Ris.Domain.Common;

    public class GetServicesByParamsCmdContext
    {
        public int ServiceId { get; set; }

        public OrderServiceAttribute[] Attributes { get; set; }
    }
}