namespace bgTeam.DataProducerService
{
    using bgTeam.Core.Helpers;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.DataProducerService.Infrastructure;
    using bgTeam.DataProducerService.NCron;
    using bgTeam.Extensions;
    using bgTeam.Plugins;
    using bgTeam.ProduceMessages;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    internal class Runner
    {
        private readonly IAppLogger _logger;
        private readonly ISenderEntity _sender;
        private readonly ISchedulersFactory _schedulersFactory;
        private readonly IEnumerable<IPluginSchedulersFactory> _pluginSchedulersFactories;

        public Runner(
            IAppLogger logger,
            ISenderEntity sender,
            ISchedulersFactory schedulersFactory,
            IEnumerable<IPluginSchedulersFactory> pluginSchedulersFactories)
        {
            _logger = logger;
            _sender = sender;
            _schedulersFactory = schedulersFactory;
            _pluginSchedulersFactories = pluginSchedulersFactories;
        }

        public void Run()
        {
            var groups = new List<string>();

            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var configFolderPath = Path.Combine(fileInfo.Directory.FullName, "config");

            var configurations = ConfigHelper.Init<DictionaryConfig>(configFolderPath);
            _logger.Info($"find configurate - {configurations.Count()}");

            foreach (var config in configurations)
            {
                if (string.IsNullOrWhiteSpace(config.DateFormatStart))
                {
                    _logger.Error($"Cron date format not found for {config.EntityType} type");
                }
                else
                {
                    if (_pluginSchedulersFactories != null &&
                        _pluginSchedulersFactories.Any(x => x.EntityType == config.EntityType || (!string.IsNullOrWhiteSpace(config.GroupName) && x.GroupName == config.GroupName)))
                    {
                        var factory = _pluginSchedulersFactories.SingleOrDefault(x => x.EntityType == config.EntityType);
                        if (factory != null)
                        {
                            factory.Create(config);
                        }
                        else
                        {
                            factory = _pluginSchedulersFactories.Single(x => x.GroupName == config.GroupName);
                            if (!groups.Contains(config.GroupName))
                            {
                                var configs = configurations.Where(x => x.GroupName == config.GroupName).ToArray();
                                factory.CreateGroup(configs);
                                groups.Add(config.GroupName);
                            }
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(config.GroupName))
                    {
                        if (!groups.Contains(config.GroupName))
                        {
                            var configs = configurations.Where(x => x.GroupName == config.GroupName).ToArray();
                            _schedulersFactory.CreateGroup(configs);
                            groups.Add(config.GroupName);
                        }
                    }
                    else
                    {
                        _schedulersFactory.Create(config);
                    }
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
