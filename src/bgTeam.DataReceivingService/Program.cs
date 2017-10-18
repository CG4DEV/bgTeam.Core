namespace bgTeam.DataReceivingService
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore;
    using bgTeam.DataProducerService.Infrastructure;
    using bgTeam.DataReceivingService.QueryProviders.Factory;
    using bgTeam.DataReceivingService.QueryProviders.Processor;
    using bgTeam.Extensions;
    using bgTeam.Infrastructure;
    using bgTeam.Plugins;
    using bgTeam.ProcessMessages;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using bgTeam.Infrastructure.DataAccess;
    using bgTeam.Core.Helpers;

    class Program
    {
        static void Main(string[] args)
        {
            var cmdParams = CommandLineHelper.ParseArgs(args);
            var container = WindsorConfigure(cmdParams);

            container.Resolve<Runner>().Run();
        }

        public static WindsorContainer WindsorConfigure(Dictionary<string, string> args)
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();

            if (args.ContainsKey("env"))
            {
                // Задаём конфигурацию в ручную.
                container.Register(Component.For<IAppConfiguration>().ImplementedBy<AppConfigurationDefault>()
                    .DependsOn(Dependency.OnValue("name", "appsettings"))
                    .DependsOn(Dependency.OnValue("envVariable", args["env"]))
                    .LifestyleSingleton());
            }
            else
            {
                // Задаём конфигурацию через переменную среды
                container.Register(Component.For<IAppConfiguration>().ImplementedBy<AppConfigurationDefault>().LifestyleSingleton());
            }

            var appConf = container.Resolve<IAppConfiguration>();

            var queue = appConf["ReceivingQueue"];
            var demandQueue = appConf["DemandQueue"];
            var threadCount = int.Parse(appConf["ThreadCount"]);

            var queues = new[] { queue };
            var errorQueues = new[] { queue + ".Errors" };
            var demandQueues = new[] { demandQueue };
            var notificationQueues = new[] { demandQueue + ".Notification" };

            container.Register(
                Component.For<IAppLoggerConfig>().ImplementedBy<SerilogConfig>().LifestyleSingleton(),
                Component.For<IAppLogger>().ImplementedBy<AppLoggerSerilog>().LifestyleSingleton(),
                Component.For<IConnectionSetting, IQueueProviderSettings>().ImplementedBy<AppSettings>().LifestyleSingleton(),
                Component.For<IConnectionFactory>().ImplementedBy<ConnectionFactoryMsSql>().LifestyleSingleton(),
                Component.For<IRepository>().ImplementedBy<RepositoryDapper>(),
                Component.For<IQueryFactory>().ImplementedBy<QueryFactory>().Named("Default").LifestyleSingleton(),
                Component.For<IQueueWatcher<IQueueMessage>>().ImplementedBy<QueueWatcherRabbitMQ>()
                    .DependsOn(Dependency.OnValue("threadsCount", threadCount)).LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", queues)).Named("Main").LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", errorQueues)).Named("Errors").LifestyleSingleton(),
                Component.For<IEntityMapService>().ImplementedBy<DefaultMapService>().LifestyleSingleton(),
                Component.For<IMessageProvider>().ImplementedBy<DefaultMessageProvider>().LifestyleSingleton());

            container.Register(
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>().Named("DemandQueues")
                    .DependsOn(Dependency.OnValue("queues", demandQueues)).LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>().Named("DemandNotifications")
                    .DependsOn(Dependency.OnValue("queues", notificationQueues)).LifestyleSingleton());

            container.Register(
                Component.For<ISenderEntity>().ImplementedBy<SenderEntity>().Named("MainSenderEntity")
                    .DependsOn(Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("DemandQueues")))
                    .LifestyleSingleton(),
                Component.For<ISenderEntity>().ImplementedBy<SenderEntity>().Named("NotificationSenderEntity")
                    .DependsOn(Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("DemandNotifications")))
                    .LifestyleSingleton(),
                Component.For<ISenderEntity>().ImplementedBy<SenderEntity>().Named("ErrorSenderEntity")
                    .DependsOn(Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("Errors")))
                    .LifestyleSingleton());

            IQueryFactory defaultQueryFactory = null;
            IEnumerable<IQueryFactory> pluginFactories = null;

            LoadPlugins(container);

            defaultQueryFactory = container.Resolve<IQueryFactory>("Default");
            pluginFactories = container.ResolveAll<IQueryFactory>().Except(new[] { defaultQueryFactory });

            container.Register(
                Component.For<IMessageProcessor>()
                .ImplementedBy<MessageProcessor>()
                .DependsOn(
                    Dependency.OnValue("defaultFactory", defaultQueryFactory),
                    Dependency.OnValue("factories", pluginFactories)).LifestyleSingleton(),
                Component.For<Runner>()
                .DependsOn(
                    Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("Main")),
                    Dependency.OnValue("queueProviderErrors", container.Resolve<IQueueProvider>("Errors")),
                    Dependency.OnValue("watchQueue", queue)));

            return container;
        }

        private static void LoadPlugins(WindsorContainer container)
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var pluginsFolderPath = Path.Combine(fileInfo.Directory.FullName, "plugins");

            var types = PluginLoader.Load<IQueryFactory>(pluginsFolderPath);

            types.DoForEach(x => container.Register(
                Component.For<IQueryFactory>().ImplementedBy(x).NamedAutomatically(x.FullName).DependsOn(
                    Dependency.OnValue("sender", container.Resolve<ISenderEntity>("MainSenderEntity")),
                    Dependency.OnValue("notificationSender", container.Resolve<ISenderEntity>("NotificationSenderEntity")),
                    Dependency.OnValue("errorSender", container.Resolve<ISenderEntity>("ErrorSenderEntity")))
                    .LifestyleSingleton()));
        }
    }
}
