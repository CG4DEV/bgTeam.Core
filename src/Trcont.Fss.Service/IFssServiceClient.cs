namespace Trcont.Fss.Service
{
    using System.Threading.Tasks;
    using Trcont.Fss.Service.Entity;

    public interface IFssServiceClient
    {
        Task<CalcTariffResponse> DoCalculateTariffAsync(CalcTariffRequestParams param);

        CalcTariffResponse DoCalculateTariff(CalcTariffRequestParams param);
    }
}
