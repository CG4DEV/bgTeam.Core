namespace Trcont.OrderRoutesCreator.Quartz.Impl
{
    using System.Collections.Generic;
    using bgTeam;
    using bgTeam.DataAccess;
    using global::Quartz;
    using Quartz;
    using Trcont.OrderRoutesCreator.Common;
    using Trcont.OrderRoutesCreator.Quartz.Jobs;
    using Trcont.RIS.Common;

    public class SchedulersFactory : Quartz.ISchedulersFactory
    {
        private readonly IAppLogger _logger;
        private readonly IRepository _repository;
        private readonly IConnectionFactory _factory;
        private readonly ISchedulerFactory _schedulerFactory;
        private readonly IOrderRoutesCreatorService _routesCreatorService;

        public SchedulersFactory(
            IAppLogger logger,
            IRepository repository,
            IConnectionFactory factory,
            ISchedulerFactory schedulerFactory,
            IOrderRoutesCreatorService routesCreatorService)
        {
            _logger = logger;
            _repository = repository;
            _factory = factory;
            _schedulerFactory = schedulerFactory;
            _routesCreatorService = routesCreatorService;
        }

        public IScheduler Create(QuartzConfig config)
        {
            IJobDetail jobDetail = CreateJob<OrdersRoutesCreatorJob>(config.Name, CreateMap(config));
            ITrigger trigger = CreateTrigger(config.Name, config.DateFormatStart, jobDetail);
            return CreateScheduler(jobDetail, trigger);
        }

        public IEnumerable<IScheduler> GetAllSchedulers()
        {
            return _schedulerFactory.GetAllSchedulers().Result;
        }

        private IDictionary<string, object> CreateMap(QuartzConfig config)
        {
            return new Dictionary<string, object>()
            {
                { "Name", config.Name },
                { "Sql", config.SqlString },
                { "DateChangeOffset", config.DateChangeOffset },
                { "Logger", _logger },
                { "Repository", _repository },
                { "ConnectionFactory", _factory },
                { "Pool", config.Pool },
                { "RoutesCreator", _routesCreatorService }
            };
        }

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
