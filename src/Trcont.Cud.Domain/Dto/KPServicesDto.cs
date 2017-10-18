namespace Trcont.Cud.Domain.Dto
{
    using Trcont.Domain.Entity;

    public class KPServicesDto : KpServices
    {
        public string ParamPlaceFromStr { get; set; }

        public string ParamPlaceToStr { get; set; }

        public string TerritoryStr { get; set; }

        public string TariffCurrencyStr { get; set; }

        public string RzdCargoSender { get; set; }
    }
}
