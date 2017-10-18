namespace bgTeam.DataProducerCore.SchedulersFactory
{
    using System.Collections.Generic;
    using Quartz;

    public abstract class AbstractSchedulersFactory
    {
        protected readonly ISchedulerFactory _schedulerFactory;

        public AbstractSchedulersFactory(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
        }

        protected IJobDetail CreateJob<T>(string jobName, IDictionary<string, object> param)
            where T : IJob
        {
            var map = new JobDataMap(param);
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .SetJobData(map)
                .WithIdentity(jobName)
                .Build();

            return jobDetail;
        }

        protected ITrigger CreateTrigger(string triggerName, string cronString, IJobDetail job)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(job)
                .WithCronSchedule(cronString)
                .WithIdentity(triggerName)
                .StartNow()
                .Build();

            return trigger;
        }

        protected IScheduler CreateScheduler(IJobDetail job, ITrigger trigger)
        {
            IScheduler scheduler = _schedulerFactory.GetScheduler().Result;
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();

            return scheduler;
        }
    }
}
