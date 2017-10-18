namespace Trcont.Fss.Service.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CalcTariffResponse
    {
        public Guid Id { get; set; }

        public IEnumerable<DeliveryRoute> DeliveryRouteList { get; set; }
    }

    public class DeliveryRoute
    {
        public DeliveryRoute()
        {
            DeliveryArmList = new List<DeliveryArm>();
        }

        public double Priority { get; set; }

        public string Label { get; set; }

        public int Duration { get; set; }

        public string SpecTaxDescription { get; set; }

        public string SpecTaxDocumentLink { get; set; }

        public IEnumerable<DeliveryArm> DeliveryArmList { get; set; }

        //////////////////////////////////////////////////////////////////////////////

        public DateTime ActualPeriodStart { get; set; }

        public DateTime ActualPeriodEnd { get; set; }

        public decimal Summ { get; set; }

        public decimal SummVAT { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyTitle { get; set; }

        public int Color { get; set; }
    }

    public class DeliveryArm
    {
        public DeliveryArm()
        {
            ServiceMandatoryList = new List<ServiceMandatory>();
            ServiceRecommendedList = new List<ServiceRecommended>();
            ExtraServiceList = new List<ExtraService>();
            FeeList = new List<Fee>();
        }

        public string Label { get; set; }

        public int Duration { get; set; }

        public int Lenght { get; set; }

        public string FromPointCode { get; set; }

        public string ToPointCode { get; set; }

        public IEnumerable<ServiceMandatory> ServiceMandatoryList { get; set; }

        public IEnumerable<ServiceRecommended> ServiceRecommendedList { get; set; }

        public IEnumerable<ExtraService> ExtraServiceList { get; set; }

        ///////////////////////////////////////////////////////////////////////////////////////////
        public IEnumerable<Fee> FeeList { get; set; }

        public Guid FromPointGUID { get; set; }

        public Guid ToPointGUID { get; set; }
    }

    public abstract class Service
    {
        public string USLCode { get; set; }

        public decimal Cost { get; set; }

        public int Currency { get; set; }

        public int TariffType { get; set; }

        public int StdVolume { get; set; }

        public int Territory { get; set; }

        public string RenderPlace { get; set; }

        public string UseConditions { get; set; }

        // Моя самодеятельность на основе ответа от FSS

        public string ContractRegNum { get; set; }

        public DateTime? ActualPeriodStart { get; set; }

        public DateTime? ActualPeriodEnd { get; set; }

        //////////////////////////////////////////////////////////////

        public int Required { get; set; }

        public int SemiRequired { get; set; }

        public int DServiceId { get; set; }

        public int DArmId { get; set; }
    }

    public class ServiceMandatory : Service
    {
        public ServiceMandatory()
        {
            Required = 1;
            SemiRequired = 0;
        }
    }

    public class ServiceRecommended : Service
    {
        public ServiceRecommended()
        {
            Required = 1;
            SemiRequired = 1;
        }
    }

    public class ExtraService : Service
    {
        public ExtraService()
        {
            Required = 0;
            SemiRequired = 0;
        }
    }

    public class Fee
    {
        public decimal LowerLimit { get; set; }

        public decimal UpperLimit { get; set; }

        public string USLCode { get; set; }

        public int Currency { get; set; }

        /////////////////////////////////////////////////////////////////////

        public int DFeeId { get; set; }

        public int DArmId { get; set; }
    }
}
