namespace bgTeam.Impl.Quartz.Impl
{
    using bgTeam.Extensions;
    using global::Quartz;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Schedulers factory Quartz
    /// </summary>
    public abstract class BaseSchedulersFactory : ISchedulersFactory
    {
        private readonly IServiceProvider _container;
        private readonly ISchedulerFactory _schedulerFactory;

        protected BaseSchedulersFactory(
            IServiceProvider container,
            ISchedulerFactory schedulerFactory)
        {
            _container = container;
            _schedulerFactory = schedulerFactory;
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
            var list = _schedulerFactory.GetAllSchedulers().Result;

            if (!list.NullOrEmpty())
            {
                list.DoForEach(x => x.Shutdown(false));
            }
        }

        protected abstract IDictionary<string, object> CreateCommonMap(IJobTriggerInfo config);

        //private IDictionary<string, object> CreateCommonMap(InsuranceConfiguration config)
        //{
        //    var logger = _container.GetService<IAppLogger>();
        //    var repository = _container.GetService<IRepository>();
        //    var sender = _container.GetService<ISenderEntityTest>();

        //    var sqlObject = new SqlObjectDefault(
        //        config.SqlString,
        //        new
        //        {
        //            DateChangeFrom = config.DateChangeOffsetFrom.HasValue ? DateTime.Now.AddHours(config.DateChangeOffsetFrom.Value) : new DateTime(1900, 01, 01),
        //            DateChangeTo = config.DateChangeOffsetTo.HasValue ? DateTime.Now.AddHours(config.DateChangeOffsetTo.Value) : new DateTime(1900, 01, 01),
        //        });

        //    return new Dictionary<string, object>()
        //    {
        //        { "Logger", logger },
        //        { "Repository", repository },
        //        { "Sender", sender },

        //        { "ContextType", config.ContextType },
        //        { "QueueName", config.NameQueue },
        //        { "SqlObject", sqlObject },
        //    };
        //}

        private IJobDetail CreateJob<T>(string jobName, IDictionary<string, object> param)
            where T : IJob
        {
            var map = new JobDataMap(param);
            IJobDetail jobDetail = JobBuilder.Create<T>()
                .SetJobData(map)
                .WithIdentity(jobName)
                .Build();

            return jobDetail;
        }

        private ITrigger CreateTrigger(string triggerName, string cronString, IJobDetail job)
        {
            ITrigger trigger = TriggerBuilder.Create()
                .ForJob(job)
                .WithCronSchedule(cronString)
                .WithIdentity(triggerName)
                .StartNow()
                .Build();

            return trigger;
        }

        private IScheduler CreateScheduler(IJobDetail job, ITrigger trigger)
        {
            IScheduler scheduler = _schedulerFactory.GetScheduler().Result;
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();

            return scheduler;
        }
    }
}
