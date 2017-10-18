namespace Trcont.CitTrans.Service
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Trcont.CitTrans.Service.Entity;

    public interface ICitTransServiceClient
    {
        Task<IEnumerable<DislocationInfo>> GetOrderDislokAsync(string orderNumber, string containerNumber);
    }
}
