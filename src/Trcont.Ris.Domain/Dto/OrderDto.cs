namespace Trcont.Ris.Domain.Dto
{
    using System;
    using Trcont.Ris.Domain.Entity;

    public class OrderDto : Order
    {
        public string ContractNumber { get; set; }

        public int ContainerTypeId { get; set; }

        public string ContainerTypeTitle { get; set; }

        public string TransTypeTitle { get; set; }


        public string PlaceFromTitle { get; set; }

        public string PlaceFromCnsiCode { get; set; }

        public Guid? PlaceFromCnsiGuid { get; set; }

        public string CountryFromTitle { get; set; }

        public string CountryFromCode { get; set; }




        public string PlaceToTitle { get; set; }

        public string PlaceToCnsiCode { get; set; }

        public Guid? PlaceToCnsiGuid { get; set; }

        public string CountryToTitle { get; set; }

        public string CountryToCode { get; set; }



        public string EtsngTitle { get; set; }

        public string EtsngCode { get; set; }

        public string GngTitle { get; set; }

        public string GngCode { get; set; }


        public string ClientName { get; set; }

        public Guid? ClientGuid { get; set; }


        public string CurrencyTitle { get; set; }

        public string CurrencyCode { get; set; }


        public string StatusTitle { get; set; }


        public decimal Summ { get; set; }

        public DateTime? SendDate { get; set; }

        public DateTime? ArrivalDate { get; set; }

        public DateTime? LoadDate { get; set; }
    }
}
