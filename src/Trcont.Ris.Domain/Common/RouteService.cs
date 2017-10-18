namespace Trcont.Ris.Domain.Common
{
    using System;
    using System.Collections.Generic;
    using Trcont.Domain.Common;

    public class RouteService
    {
        public int? ServiceId { get; set; }

        public string ServiceCode { get; set; }

        public int? ServiceFortId { get; set; }

        public string TerritoryCode { get; set; }

        public Guid? TerritoryGuid { get; set; }

        public string Title { get; set; }

        public int? CurrencyCode { get; set; }

        public string CurrencyTitle { get; set; }

        public decimal Summ { get; set; }

        public decimal SummVAT { get; set; }

        public decimal Tariff { get; set; }

        public decimal TariffVAT { get; set; }

        public int TariffType { get; set; }

        public decimal SrcVolume { get; set; }

        public string ContractRegNum { get; set; }

        public bool IsAutoFromExist { get; set; }

        public bool IsAutoToExist { get; set; }

        public ServiceTypeEnum ServiceType { get; set; }

        //////////////////////////////////////////////////////////////

        public Guid RefServiceTypeGuid { get; set; }

        public Guid RefDocumentGuid { get; set; }

        public Guid TariffCurrencyGuid { get; set; }

        public Guid? ParamTypeContGuid { get; set; }

        public Guid? ParamZoneFromGuid { get; set; }

        public Guid? ParamZoneToGuid { get; set; }

        public Guid? ParamPortToGuid { get; set; }

        public Guid? ParamPortFromGuid { get; set; }

        public Guid? ParamSenderGuid { get; set; }

        public Guid? ParamReceiverGuid { get; set; }

        public Guid? ParamPortToAgentGuid { get; set; }

        public Guid? ParamPortFromAgentGuid { get; set; }

        public string ParamStockFromAddress { get; set; }

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

        public Guid? ParamPowerSet { get; set; }

        public Guid? ParamAddCondOrgSvc { get; set; }

        public Guid? ParamAddCondTermFrom { get; set; }

        public Guid? ParamAddCondTermTo { get; set; }

        public int PayAccId { get; set; }

        public Guid? ParamContState { get; set; }

        public int? SourceReferenceId { get; set; }

        public int? SourcePriceServiceId { get; set; }

        public Guid? ParamCargoKindGuid { get; set; }

        public Guid? ParamTerritoryGuid { get; set; }

        public Guid? ParamTransTypeGuid { get; set; }

        public Guid? ParamDocDeliveryTypeGuid { get; set; }

        public Guid? ParamZPUTypeGuid { get; set; }

        public int IsExternalRate { get; set; }

        public Guid? ContractGuid { get; set; }

        public Guid? ExecGuid { get; set; }

        public float? Duration { get; set; }

        public int ArmIndex { get; set; }

        public Guid? ParamMeasureGuid { get; set; }

        public Guid? ParamSchemeLoadingGuid { get; set; }

        public string CargoSender { get; set; }

        public string CargoReciever { get; set; }

        public string CargoSender2 { get; set; }

        public string CargoReciever2 { get; set; }

        public string PortFromAgent { get; set; }

        public string PortToAgent { get; set; }

        public string RzdCargoSender { get; set; }

        public bool ServiceForPoint { get; set; }
    }
}
