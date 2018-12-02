namespace bgTeam.Impl.Quartz
{
    using global::Quartz;
    using System;
    using System.Collections.Generic;

    public interface ISchedulersFactory : IDisposable
    {
        void Create<T>(IJobTriggerInfo config)
            where T : IJob;
    }
}
