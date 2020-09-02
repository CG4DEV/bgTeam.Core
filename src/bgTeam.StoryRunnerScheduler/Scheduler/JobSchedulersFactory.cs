namespace bgTeam.StoryRunnerScheduler.Scheduler
{
    using System;
    using System.Collections.Generic;
    using bgTeam.DataAccess;
    using bgTeam.Impl.Quartz;
    using bgTeam.Quartz;
    using bgTeam.Queues;
    using global::Quartz;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Schedulers factory Quartz
    /// </summary>
    public class JobSchedulersFactory : BaseSchedulersFactory
    {
        public JobSchedulersFactory(
            IServiceProvider container,
            ISchedulerFactory schedulerFactory)
            : base(container, schedulerFactory)
        {
        }

        protected override IDictionary<string, object> CreateCommonMap(IJobTriggerInfo conf)
        {
            var logger = _container.GetService<IAppLogger>();
            var repository = _container.GetService<IRepository>();
            var sender = _container.GetService<ISenderEntity>();

            return new Dictionary<string, object>
            {
                { "Logger", logger },
                { "Repository", repository },
                { "Sender", sender },
                { "Config", conf },
            };
        }
    }
}
