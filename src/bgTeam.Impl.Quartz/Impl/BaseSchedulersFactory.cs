namespace bgTeam.Impl.Quartz
{
    using System;
    using System.Collections.Generic;
    using bgTeam.Extensions;
    using bgTeam.Quartz;
    using global::Quartz;

    /// <summary>
    /// Schedulers factory Quartz
    /// </summary>
    public abstract class BaseSchedulersFactory : ISchedulersFactory
    {
        protected readonly IServiceProvider _container;
        private readonly ISchedulerFactory _schedulerFactory;

        protected BaseSchedulersFactory(
            IServiceProvider container,
            ISchedulerFactory schedulerFactory)
        {
            _container = container;
            _schedulerFactory = schedulerFactory ?? throw new ArgumentNullException(nameof(schedulerFactory));
        }

        public void Create<T>(IJobTriggerInfo config)
            where T : IJob
        {
            IJobDetail jobDetail = CreateJob<T>(config.Name, CreateCommonMap(config));
            ITrigger trigger = CreateTrigger(config.Name, config.DateStart, jobDetail);

            CreateScheduler(jobDetail, trigger);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract IDictionary<string, object> CreateCommonMap(IJobTriggerInfo config);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var list = _schedulerFactory.GetAllSchedulers().Result;

                if (!list.NullOrEmpty())
                {
                    list.DoForEach(x => x.Shutdown(false));
                }
            }
        }

        private static IJobDetail CreateJob<T>(string jobName, IDictionary<string, object> param)
            where T : IJob
        {
            var map = new JobDataMap(param);
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .SetJobData(map)
                .WithIdentity(jobName)
                .Build();

            return jobDetail;
        }

        private static ITrigger CreateTrigger(string triggerName, string cronString, IJobDetail job)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(job)
                .WithCronSchedule(cronString)
                .WithIdentity(triggerName)
                .StartNow()
                .Build();

            return trigger;
        }

        private void CreateScheduler(IJobDetail job, ITrigger trigger)
        {
            IScheduler scheduler = _schedulerFactory.GetScheduler().Result;
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }
    }
}
