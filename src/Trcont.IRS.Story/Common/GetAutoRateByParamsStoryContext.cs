namespace Trcont.IRS.Story.Common
{
    using System;

    public class GetAutoRateByParamsStoryContext
    {
        public Guid? ZoneGuid { get; set; }

        public Guid? PointGuid { get; set; }

        public Guid? ContainerTypeGuid { get; set; }
    }
}