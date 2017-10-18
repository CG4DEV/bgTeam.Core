namespace Trcont.OrderRoutesCreator
{
    using bgTeam;
    using bgTeam.Core.Helpers;
    using System.Linq;
    using System.IO;
    using System.Reflection;
    using Trcont.OrderRoutesCreator.Common;
    using Trcont.OrderRoutesCreator.Quartz;
    using bgTeam.Extensions;

    public class Runner
    {
        private readonly IAppLogger _logger;
        private readonly ISchedulersFactory _schedulersFactory;

        public Runner(
            IAppLogger logger,
            ISchedulersFactory schedulersFactory)
        {
            _logger = logger;
            _schedulersFactory = schedulersFactory;
        }

        public void Run()
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var configFolderPath = Path.Combine(fileInfo.Directory.FullName, "QuartzConfigs");
            var configurations = ConfigHelper.Init<QuartzConfig>(configFolderPath);
            _logger.Info($"find configurate - {configurations.Count()}");

            foreach (var config in configurations)
            {
                if (string.IsNullOrWhiteSpace(config.DateFormatStart))
                {
                    _logger.Error($"Cron date format not found for {config.Name} config");
                }
                else
                {
                    _schedulersFactory.Create(config);
                }
            }
        }

        public void Stop()
        {
            var collection = _schedulersFactory.GetAllSchedulers();

            if (collection != null && collection.Any())
            {
                collection.DoForEach(x => x.Shutdown(false));
            }
        }
    }
}
