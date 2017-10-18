namespace Trcont.Fss.Service.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.Extensions;

    public class CalcTariffRequestParams
    {
        public int ContOwner { get; set; }

        public int ContPark { get; set; }

        public string ContTypeCode { get; set; }

        public DateTime Date { get; set; }

        public string ETSNGcode { get; set; }

        public string FromPointCode { get; set; }

        public string GNGCode { get; set; }

        public bool IsСontTrain { get; set; }

        public double NettoWeight { get; set; }

        public int Rank { get; set; }

        public int Speed { get; set; }

        public string ToPointCode { get; set; }

        public double TotalWeight { get; set; }

        public int WagonOwner { get; set; }

        public int WagonPark { get; set; }

        public string WagonTypeCode { get; set; }

        public decimal DeclaredCost { get; set; }
    }
}
