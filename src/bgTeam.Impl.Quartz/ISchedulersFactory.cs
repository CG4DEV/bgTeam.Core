namespace bgTeam.Impl.Quartz
{
    using global::Quartz;
    using System;

    public interface ISchedulersFactory : IDisposable
    {
        void Create<T>(IJobTriggerInfo config)
            where T : IJob;
    }
}
