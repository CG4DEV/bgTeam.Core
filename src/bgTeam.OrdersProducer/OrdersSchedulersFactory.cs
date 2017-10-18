namespace bgTeam.OrdersProducer
{
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.DataProducerCore.SchedulersFactory;
    using bgTeam.OrdersProducer.Jobs;
    using bgTeam.ProduceMessages;
    using Quartz;
    using System;
    using System.Collections.Generic;

    public class OrdersSchedulersFactory : AbstractSchedulersFactory, IPluginSchedulersFactory
    {
        private readonly IAppLogger _logger;
        private readonly IRepositoryData _repository;
        private readonly ISenderEntity _sender;
        private readonly ISenderEntity _notificationSender;

        public OrdersSchedulersFactory(
            IAppLogger logger,
            IRepositoryData repository,
            ISenderEntity sender,
            ISenderEntity notificationSender,
            ISchedulerFactory schedulerFactory)
            : base(schedulerFactory)
        {
            _logger = logger;
            _repository = repository;
            _sender = sender;
            _notificationSender = notificationSender;
        }

        public string EntityType => null;

        public string GroupName => "Orders";

        public void Create(IDictionaryConfig config)
        {
            throw new NotSupportedException();
        }

        public void CreateGroup(IEnumerable<IDictionaryConfig> configs)
        {
            var group = new GroupConfig(configs);

            IJobDetail jobDetail = CreateJob<OrdersJob>(group.GroupName, CreateGroupMap(group));
            ITrigger trigger = CreateTrigger(group.GroupName, group.DateFormatStart, jobDetail);
            CreateScheduler(jobDetail, trigger);
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
