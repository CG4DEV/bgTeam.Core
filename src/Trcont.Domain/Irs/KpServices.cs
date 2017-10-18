namespace Trcont.Domain.Entity
{
    using System;

    public class KpServices
    {
        public Guid ServiceGuid { get; set; }

        public Guid RefServiceTypeGuid { get; set; }

        public Guid RefDocumentGuid { get; set; }

        public string ServiceTitle { get; set; }

        public decimal Tariff { get; set; }

        public Guid TariffCurrencyGuid { get; set; }

        public decimal Summ { get; set; }

        public Guid? ParamPlaceFromGuid { get; set; }

        public Guid? ParamPlaceToGuid { get; set; }

        public Guid? ParamPlaceGuid { get; set; }

        public Guid? ParamTypeContGuid { get; set; }

        public Guid? ParamZoneFromGuid { get; set; }

        public float? ParamZoneFromKm { get; set; }

        public Guid? ParamZoneToGuid { get; set; }

        public float? ParamZoneToKm { get; set; }

        public float? ParamFromKm { get; set; }

        public float? ParamToKm { get; set; }

        public float? ParamFromTime { get; set; }

        public float? ParamToTime { get; set; }

        public Guid? ParamPortToGuid { get; set; }

        public Guid? ParamPortFromGuid { get; set; }

        public Guid? ParamSenderGuid { get; set; }

        public Guid? ParamReceiverGuid { get; set; }

        public Guid? ParamPortToAgentGuid { get; set; }

        public Guid? ParamPortFromAgentGuid { get; set; }

        public string ParamStockToAddress { get; set; }

        public string ParamStockFromAddress { get; set; }

        public float Volume { get; set; }

        public Guid? ParamZoneFrom2Guid { get; set; }

        public Guid? ParamZoneTo2Guid { get; set; }

        public string ParamStockToAddress2 { get; set; }

        public string ParamStockFromAddress2 { get; set; }

        public Guid? ParamSender2Guid { get; set; }

        public Guid? ParamReceiver2Guid { get; set; }

        public Guid? AddConditionFromGuid { get; set; }

        public Guid? AddConditionToGuid { get; set; }

        public Guid? CarTypeGuid { get; set; }

        public int? ParamCustomMode { get; set; }

        public DateTime? LoadDate { get; set; }

        public DateTime? UnLoadDate { get; set; }

        public string PeopleStockTo { get; set; }

        public string PeopleStockFrom { get; set; }

        public string PeopleStockTo2 { get; set; }

        public string PeopleStockFrom2 { get; set; }

        public Guid? VolumeKuzovGuid { get; set; }

        public Guid? TonnagGuid { get; set; }

        public Guid? PackageGuid { get; set; }

        public Guid? GCargoGuid { get; set; }

        public Guid? JarusGuid { get; set; }

        public Guid? KoefRGuid { get; set; }

        public decimal? Tariff2 { get; set; }

        public Guid? ExecutorGuid { get; set; }

        public float? ExecVolume { get; set; }

        public Guid? ExecVolumeMeasureGuid { get; set; }

        public float? ExecVolume2 { get; set; }

        public Guid? ExecVolumeMeasure2Guid { get; set; }

        public float? Volume2 { get; set; }

        public Guid? VolumeMeasureGuid { get; set; }

        public Guid? VolumeMeasure2Guid { get; set; }

        public Guid? SrvPlaceFromGuid { get; set; }

        public Guid? SrvPlaceToGuid { get; set; }

        public Guid? AddressStockFromGuid { get; set; }

        public Guid? AddressStockToGuid { get; set; }

        public Guid? AddressStock2FromGuid { get; set; }

        public Guid? AddressStock2ToGuid { get; set; }

        public Guid? ParkKontGuid { get; set; }

        public Guid? ZakIspGuid { get; set; }

        public Guid? ParamPowerSet { get; set; }

        public Guid? ParamAddCondOrgSvc { get; set; }

        public Guid? ParamAddCondTermFrom { get; set; }

        public Guid? ParamAddCondTermTo { get; set; }

        public int PayAccId { get; set; }

        public decimal? EstimateTariff { get; set; }

        public Guid? ParamContState { get; set; }

        public int? SourceReferenceId { get; set; }

        public int? SourcePriceServiceId { get; set; }

        public Guid? ParamCargoKindGuid { get; set; }

        public Guid? ParamTerritoryGuid { get; set; }

        public Guid? ParamTransTypeGuid { get; set; }

        public Guid? ParamDocDeliveryTypeGuid { get; set; }

        public Guid? ParamZPUTypeGuid { get; set; }

        public int IsRequired { get; set; }

        public int IsExternalRate { get; set; }

        public int IsActive { get; set; }

        public Guid? TerritoryGuid { get; set; }

        public Guid? ContractGuid { get; set; }

        public Guid? ExecGuid { get; set; }

        public DateTime? ActualPeriodStart { get; set; }

        public DateTime? ActualPeriodEnd { get; set; }

        public float? PathLength { get; set; }

        public float? Duration { get; set; }

        public int ArmIndex { get; set; }

        public int TariffType { get; set; }

        public decimal TariffVAT { get; set; }

        public decimal SummVAT { get; set; }

        public decimal ConvertRate { get; set; }

        public string Notes { get; set; }

        public Guid? ParamMeasureGuid { get; set; }

        public Guid? ParamInsulationGuid { get; set; }

        public Guid? ParamLiftTypeGuid { get; set; }

        public Guid? ParamSchemeLoadingGuid { get; set; }

        public string CargoSender { get; set; }

        public string CargoReciever { get; set; }
    }
}
