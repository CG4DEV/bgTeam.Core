namespace Trcont.Domain.Entity
{
    using DapperExtensions.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class KpDocuments
    {
        [PrymaryKey]
        public Guid ReferenceGuid { get; set; }

        public Guid UserGuid { get; set; }

        public Guid? ClientGuid { get; set; }

        public Guid? ContractGuid { get; set; }

        public Guid? TeoGuid { get; set; }

        public string Number { get; set; }

        public string Comments { get; set; }

        public Guid PlaceFromGuid { get; set; }

        public Guid PlaceToGuid { get; set; }

        public Guid CountryFromGuid { get; set; }

        public Guid CountryToGuid { get; set; }

        public Guid SendingGuid { get; set; }

        public Guid TrainTypeGuid { get; set; }

        public int ContainerQuantity { get; set; }

        public int ContOwner { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public Guid ETSNGGuid { get; set; }

        public Guid GNGGuid { get; set; }

        public float Weight { get; set; }

        public int Speed { get; set; }

        public int Status { get; set; }

        public DateTime AccessDate { get; set; }

        public int? EditCompletedStatus { get; set; }

        public Guid? StationOutGuid { get; set; }

        public Guid? StationInGuid { get; set; }

        public int WagonPark { get; set; }

        public int TransType { get; set; }

        public int TransTypeId { get; set; }

        public int OutCategory { get; set; }

        public int RoutType { get; set; }

        public float? TransLenRoad { get; set; }

        public int DangerClass { get; set; }

        public int DangerSubClass { get; set; }

        public int TemplateId { get; set; }

        public float WeightBrutto { get; set; }

        public Guid CurrencyGuid { get; set; }

        public Guid? PrevLoadETSNGGuid { get; set; }

        public int? RefSectionSize { get; set; }

        public int? SignedTK { get; set; }

        public int? SignedClient { get; set; }

        public DateTime StatusDate { get; set; }

        public string ClientName { get; set; }

        public string ContactFace { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEMail { get; set; }

        public bool ContrRelationsType { get; set; }

        public string DocumentTitle { get; set; }

        public string ConditionDescription { get; set; }

        public string ConditionDocLink { get; set; }

        public string ExternalXml { get; set; }

        public float? Duration { get; set; }

        public int? ChosenRouteNum { get; set; }

        public int IsExternalRate { get; set; }

        public bool CustomType { get; set; }

        public int RefreshExchRates { get; set; }

        public string TransAccount { get; set; }

        public int DeleteStatus { get; set; }
    }
}
