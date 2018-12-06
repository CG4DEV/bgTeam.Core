namespace bgTeam.Quartz
{
    using global::Quartz;
    using System;

    public interface ISchedulersFactory : IDisposable
    {
        void Create<T>(IJobTriggerInfo config)
            where T : IJob;
    }
}
