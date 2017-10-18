namespace Trcont.Ris.Domain.Dto
{
    using System;
    using System.Collections.Generic;
    using Trcont.Ris.Domain.Entity;

    public class ServiceParamDto : MinServiceParamDto
    {
        public int ServiceId { get; set; }

        public Guid ServiceGuid { get; set; }

        public string ServiceName { get; set; }

        public string ServiceCode { get; set; }

        //public ServiceParamDto Clone()
        //{
        //    return new ServiceParamDto()
        //    {
        //        ParamGuid = this.ParamGuid,
        //        ParentGuid = this.ParentGuid,
        //        ParamType = this.ServiceId,
        //        Code = this.ServiceId,
        //        NameRus = this.NameRus,
        //        Description = this.Description,
        //        DomainGuid = this.DomainGuid,
        //        DomainName = this.DomainName,
        //        IsEditAllow = this.ServiceId,
        //        IsMultiValues = this.ServiceId,
        //        IsVisible = this.ServiceId,
        //        IsEditUserValue = this.IsEditUserValue,
        //        ServiceId = this.ServiceId,
        //        ServiceGuid = this.ServiceGuid,
        //        ServiceName = this.ServiceName
        //    };
        //}
    }
}
