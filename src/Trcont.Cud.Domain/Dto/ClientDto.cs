namespace Trcont.Cud.Domain.Dto
{
    using System;

    public class ClientDto
    {
        public Guid ClientGuid { get; set; }

        public int ClientType { get; set; }

        public string JCompanyName { get; set; }

        public string Kpp { get; set; }

        public string JIndex { get; set; }

        public Guid? JCountryGUID  { get; set; }

        public string JCountryTitle { get; set; }

        public string JCity { get; set; }

        public string JRegion { get; set; }

        public string JAddress { get; set; }

        public string FPhone { get; set; }

        public string FFax { get; set; }

        public string FEmail { get; set; }

        public string Inn { get; set; }

        public string FSecondName { get; set; }

        public string FFirstName { get; set; }

        public string FMiddleName { get; set; }

        public string DocSeries { get; set; }

        public string DocNumber { get; set; }

        public DateTime? DocDataReceive { get; set; }

        public string DocIssuer { get; set; }

        public string FIndex { get; set; }

        public Guid? FCountryGUID { get; set; }

        public string FCountryTitle { get; set; }

        public string FCity { get; set; }
        
        public string FRegion { get; set; }

        public string FAddress { get; set; }
        
        public string Okpo { get; set; }

        public string Okato { get; set; }

        public string Okopf { get; set; }

        public string Okfc { get; set; }

        public string JShortName { get; set; }
    }
}
