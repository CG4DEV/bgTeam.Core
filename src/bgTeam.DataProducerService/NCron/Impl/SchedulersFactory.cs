namespace bgTeam.DataProducerService.NCron.Impl
{
    using System.Collections.Generic;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.DataProducerService.NCron.Jobs;
    using bgTeam.ProduceMessages;
    using Quartz;
    using bgTeam.DataProducerCore.SchedulersFactory;

    public class SchedulersFactory : AbstractSchedulersFactory, ISchedulersFactory
    {
        private readonly IAppLogger _logger;
        private readonly IRepositoryData _repository;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        public SchedulersFactory(
            IAppLogger logger,
            IRepositoryData repository,
            ISenderEntity sender,
            ISenderEntity notificationSender,
            ISchedulerFactory schedulerFactory)
            : base (schedulerFactory)
        {
            _logger = logger;
            _repository = repository;
            _sender = sender;
            _notificationSender = notificationSender;
        }

        public IScheduler Create(DictionaryConfig config)
        {
            IJobDetail jobDetail = CreateJob<CommonJob>(config.EntityType, CreateCommonMap(config));
            ITrigger trigger = CreateTrigger(config.EntityType, config.DateFormatStart, jobDetail);
            return CreateScheduler(jobDetail, trigger);
        }

        public IScheduler CreateGroup(IEnumerable<DictionaryConfig> configs)
        {
            var group = new GroupConfig(configs);

            IJobDetail jobDetail = CreateJob<GroupJob>(group.GroupName, CreateGroupMap(group));
            ITrigger trigger = CreateTrigger(group.GroupName, group.DateFormatStart, jobDetail);
            return CreateScheduler(jobDetail, trigger);
        }

        public IEnumerable<IScheduler> GetAllSchedulers()
        {
            return _schedulerFactory.GetAllSchedulers().Result;
        }

        private IDictionary<string, object> CreateCommonMap(DictionaryConfig config)
        {
            return new Dictionary<string, object>()
            {
                { "EntityType", config.EntityType },
                { "EntityKey", config.EntityKey },
                { "DateChangeOffset", config.DateChangeOffset },
                { "Sql", config.SqlString },
                { "Logger", _logger },
                { "Repository", _repository },
                { "Sender", _sender },
                { "NotificationSender", _notificationSender },
                { "Pool", config.Pool }
            };
        }

        private IDictionary<string, object> CreateGroupMap(GroupConfig config)
        {
            return new Dictionary<string, object>()
            {
                { "GroupConfig", config },
                { "Logger", _logger },
                { "Repository", _repository },
                { "Sender", _sender },
                { "NotificationSender", _notificationSender }
            };
        }
    }
}
