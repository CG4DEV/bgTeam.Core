namespace Trcont.Ris.Domain.Common
{
    using System;

    public interface IBaseRoute
    {
        Guid? FromPointGUID { get; set; }

        Guid? ToPointGUID { get; set; }

        int ArmIndex { get; set; }
    }
}
