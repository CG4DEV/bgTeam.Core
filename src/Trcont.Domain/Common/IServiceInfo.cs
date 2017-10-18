namespace Trcont.Domain.Common
{
    public interface IServiceInfo
    {
        int ServiceId { get; set; }

        string ServiceCode { get; set; }

        string ServiceTitle { get; set; }

        ServiceTypeEnum ServiceType { get; set; }
    }
}
