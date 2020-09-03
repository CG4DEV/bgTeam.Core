namespace bgTeam.StoryRunnerScheduler
{
    using System.Collections.Generic;
    using bgTeam.Quartz;
    using bgTeam.StoryRunnerScheduler.Scheduler;
    using bgTeam.StoryRunnerScheduler.Scheduler.Jobs;

    public class Runner
    {
        private readonly IAppLogger _logger;
        private readonly ISchedulersFactory _schedulersFactory;
        private readonly IEnumerable<JobTriggerInfo> _configurations;

        public Runner(
            IAppLogger logger,
            ISchedulersFactory schedulersFactory,
            IEnumerable<JobTriggerInfo> configurations)
        {
            _logger = logger;
            _configurations = configurations;
            _schedulersFactory = schedulersFactory;
        }

        public void Run()
        {
            foreach (var config in _configurations)
            {
                _schedulersFactory.Create<MainJob>(config);
            }
        }

        public void Stop()
        {
            _schedulersFactory.Dispose();
        }
    }
}
