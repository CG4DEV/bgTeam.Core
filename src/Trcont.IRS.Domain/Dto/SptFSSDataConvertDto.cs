using System;
using System.Collections.Generic;

namespace Trcont.IRS.Domain.Dto
{
    public class SptFSSDataConvertDto
    {
        public IEnumerable<ErrorInfo> Errors { get; set; }

        public IEnumerable<USLCodeInfo> USLCodes { get; set; }

        public IEnumerable<ExchangeRateInfo> ExchangeRates { get; set; }

        public IEnumerable<ServiceInfo> Services { get; set; }
    }

    //select
    //errorcode,
    //errortext
    //from @error
    public class ErrorInfo
    {
        public int ErrorCode { get; set; }

        public string ErrorText { get; set; }
    }

    //--1. типы услуг
    //select
    //uslcode,
    //worktypeguid
    //from @uslcodes
    public class USLCodeInfo
    {
        public string UslCode { get; set; }

        public Guid WorkTypeGUID { get; set; }

        public int WorkTypeId { get; set; }
    }

    //--2. курсы конвертации
    //select
    //rate,
    //displayrate,
    //displaydirect,
    //convfromid,
    //convtoid
    //from @exchangerates
    public class ExchangeRateInfo
    {
        public decimal Rate { get; set; }

        public decimal DisplayRate { get; set; }

        public bool DisplayDirect { get; set; }

        public int ConvFromId { get; set; }

        public int ConvToId { get; set; }
    }


    //--3. услуги
    //select
    //darmid,
    //dserviceid,
    //contractguid,
    //execguid,
    //execdisplay,
    //territoryguid,
    //sourcecurrencyid,
    //isexecresident,
    //isnds0state,
    //isnottranscontainer
    //from @services
    public class ServiceInfo
    {
        public int DArmId { get; set; }

        public int DServiceId { get; set; }

        public Guid ContractGUID { get; set; }

        public Guid ExecGUID { get; set; }

        public string ExecDisplay { get; set; }

        public Guid TerritoryGUID { get; set; }

        public int SourceCurrencyId { get; set; }

        public int IsExecResident { get; set; }

        public int IsNds0State { get; set; }

        public int IsNotTranscontainer { get; set; }
    }
}
