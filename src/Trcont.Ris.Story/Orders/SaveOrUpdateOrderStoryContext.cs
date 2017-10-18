namespace Trcont.Ris.Story.Orders
{
    using bgTeam.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Trcont.Ris.Domain.Common;
    using Trcont.Ris.Domain.Entity;
    using Trcont.Ris.Domain.Enums;

    public class SaveOrUpdateOrderStoryContext
    {
        public Guid? ReferenceGuid { get; set; }

        public Guid UserGuid { get; set; }

        public string ClientName { get; set; }

        public string ContactFace { get; set; }

        public string ContactPhone { get; set; }

        public string ContactEMail { get; set; }

        public Guid? ClientGuid { get; set; }

        

        public Guid? ContractGuid { get; set; }

        public Guid CurrencyGuid { get; set; }

        public int? CustomsMode { get; set; }

        public DateTime PeriodBeginDate { get; set; }

        public string PeriodBeginOffset { get; set; }

        public DateTime PeriodEndDate { get; set; }

        public string PeriodEndOffset { get; set; }

        public Guid? PlaceFromGuid { get; set; }

        public Guid CountryFromGuid { get; set; }

        public Guid? PlaceToGuid { get; set; }

        

        public Guid CountryToGuid { get; set; }

        public SendingTypeEnum SendingType { get; set; }

        //public Guid SendingGuid => Guid.Parse(SendingType.GetDescription());

        /// <summary>
        /// Тип контейнера
        /// </summary>
        public Guid? TrainTypeGuid { get; set; }

        public string TrainTypeCnsi { get; set; }


        //public string ETSNGCode { get; set; }

        //public Guid ETSNGGuid { get; set; }

        //public string GNGCode { get; set; }

        //public Guid? GNGGuid { get; set; }

        public IEnumerable<OrdersCargoInfo> CargoInfo { get; set; }


        public double Weight { get; set; }

        public double WeightBrutto { get; set; }

        public OutCategoryTypeEnum OutCategory { get; set; }

        public TypeMessageEnum TypeMessage { get; set; }

        //public int RouteType { get; set; }

        //public int? RefSectionSize { get; set; }

        //public int WagonQuantity { get; set; }

        //public ContrRelationsTypeEnum ContrRelationsType { get; set; }

        /// <summary>
        /// Количество контейнеров
        /// </summary>
        public int ContainerQuantity { get; set; }

        public ContOwnerTypeEnum ContOwner { get; set; }

        public WagonParkTypeEnum WagonPark { get; set; }

        public SpeedEnum Speed { get; set; }

        public string Comments { get; set; }

        public IEnumerable<RouteForSave> Routes { get; set; }

        public string RouteId { get; set; }

        public string RouteSpecId { get; set; }

        public string ExternalXML { get; set; }


        public string CurrencyCnsi { get; set; }

        public string PlaceFromCnsi { get; set; }

        public string CountryFromCode { get; set; }

        public string PlaceToCnsi { get; set; }

        public string CountryToCode { get; set; }

        //public int ContrRelationsType { get; set; }
    }


}
