namespace Trcont.Cud.Story.Orders
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using Trcont.Cud.Domain.Common;
    using Trcont.Cud.Domain.Dto;
    using Trcont.Cud.Domain.Enum;

    public class SaveOrUpdateOrderStoryContext
    {
        public Guid? ReferenceGuid { get; set; }

        public Guid UserGuid { get; set; }

        public string ClientName { get; set; }

        public string ContactFace { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEMail { get; set; }

        public Guid? ClientGuid { get; set; }

        public Guid? ClientGuidCNSI { get; set; }

        public Guid? ContractGuid { get; set; }

        public Guid CurrencyGuid { get; set; }

        public int CustomsMode { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public Guid? PlaceFromGuid { get; set; }

        public Guid? PlaceFromGuidCNSI { get; set; }

        public Guid CountryFromGuid { get; set; }

        public Guid? PlaceToGuid { get; set; }

        public Guid? PlaceToGuidCNSI { get; set; }

        public Guid CountryToGuid { get; set; }

        public SendingTypeEnum SendingType { get; set; } = SendingTypeEnum.Type1;

        public Guid SendingGuid => Guid.Parse(SendingType.GetDescription());

        /// <summary>
        /// Тип контейнера
        /// </summary>
        public Guid? TrainTypeGuid { get; set; }

        public Guid? TrainTypeGuidCNSI { get; set; }

        public string ETSNGCode { get; set; }

        public Guid ETSNGGuid { get; set; }

        public Guid? GNGGuid { get; set; }

        public int Weight { get; set; }

        public int WeightBrutto { get; set; }

        public OutCategoryTypeEnum OutCategory { get; set; }

        //public int RouteType { get; set; }

        //public int? RefSectionSize { get; set; }

        //public int WagonQuantity { get; set; }

        //public ContrRelationsTypeEnum ContrRelationsType { get; set; }

        /// <summary>
        /// Количество контейнеров
        /// </summary>
        public int ContainerQuantity { get; set; }

        public ContOwnerTypeEnum ContOwner { get; set; }

        public ContOwnerTypeEnum WagonPark { get; set; }

        public SpeedEnum Speed { get; set; }

        public string Comments { get; set; }

        public IEnumerable<RouteForSave> Routes { get; set; }

        public int? RouteId { get; set; }

        public string ExternalXML { get; set; }
    }
}