namespace Trcont.OrderRoutesCreator.Quartz
{
    using System.Collections.Generic;
    using global::Quartz;
    using Trcont.OrderRoutesCreator.Common;

    public interface ISchedulersFactory
    {
        IScheduler Create(QuartzConfig config);

        IEnumerable<IScheduler> GetAllSchedulers();
    }
}
