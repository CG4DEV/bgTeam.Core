namespace Trcont.App.Service.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using TKApplicationService;

    public class OrderInfo : TeoDocumentObject
    {
        //public Guid KpGuid { get; set; }

        public Guid TransTypeGuid { get; set; }

        public new Guid? StationFromGuid
        {
            get
            {
                return base.StationFromGuid;
            }

            set
            {
                if (value == null)
                {
                    base.StationFromGuid = Guid.Empty;
                }
                else
                {
                    base.StationFromGuid = value.Value;
                }
            }
        }

        public new Guid? CountryFromGuid
        {
            get
            {
                return base.CountryFromGuid;
            }

            set
            {
                if (value == null)
                {
                    base.CountryFromGuid = Guid.Empty;
                }
                else
                {
                    base.CountryFromGuid = value.Value;
                }
            }
        }

        public new Guid? TrainTypeGuid
        {
            get
            {
                return base.TrainTypeGuid;
            }

            set
            {
                if (value == null)
                {
                    base.TrainTypeGuid = Guid.Empty;
                }
                else
                {
                    base.TrainTypeGuid = value.Value;
                }
            }
        }

        public new OrderServiceInfo[] Services { get; set; }

        public int? ContrRelationsType { get; set; }

        public int? RefreshExchRates { get; set; }

        public string ExternalXML { get; set; }

        public string PeriodBeginOffset { get; set; }

        public string PeriodEndOffset { get; set; }

        public Guid ManagerGuid { get; set; }
    }
}
