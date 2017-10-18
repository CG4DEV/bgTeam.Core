namespace bgTeam.DataProducerQueryService
{
    using System;
    using System.IO;
    using System.Reflection;
    using bgTeam.DataAccess;
    using bgTeam.DataProducerCore;
    using bgTeam.DataProducerCore.Common;
    using bgTeam.DataProducerService.Infrastructure;
    using bgTeam.Infrastructure;
    using bgTeam.ProcessMessages;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using bgTeam.Core.Impl;
    using bgTeam.Core;
    using bgTeam.Infrastructure.DataAccess;
    using bgTeam.Core.Helpers;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args) 
        {
            var cmdParams = CommandLineHelper.ParseArgs(args);
            var container = WindsorConfigure(cmdParams);

            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var configFolderPath = Path.Combine(fileInfo.Directory.FullName, "config");
            var configurations = ConfigHelper.Init<DictionaryConfig>(configFolderPath);

            container.Resolve<Runner>().Run(configurations);

            Console.WriteLine("Press any key to close service...");
            Console.ReadKey();
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

            var demandQueue = appConf["DemandQueue"];
            var producerQueue = appConf["ProducerQueue"];
            var queues = new[] { demandQueue };
            var errorQueues = new[] { demandQueue + ".Errors" };

            var responses = new[] { producerQueue };
            var responseNotifications = new[] { demandQueue + ".Notification" };

            var threadCount = int.Parse(appConf["ThreadCount"]);

            container.Register(
                Component.For<IAppLoggerConfig>().ImplementedBy<SerilogConfig>().LifestyleSingleton(),
                Component.For<IAppLogger>().ImplementedBy<AppLoggerSerilog>().LifestyleSingleton(),
                Component.For<IConnectionSetting, IQueueProviderSettings>().ImplementedBy<AppSettings>().LifestyleSingleton(),

                Component.For<IConnectionFactory>().ImplementedBy<ConnectionFactoryMsSql>().LifestyleSingleton(),
                Component.For<IRepositoryData>().ImplementedBy<RepositoryData>(),

                Component.For<IQueueWatcher<IQueueMessage>>().ImplementedBy<QueueWatcherRabbitMQ>()
                    .DependsOn(Dependency.OnValue("threadsCount", threadCount)).LifestyleSingleton(),

                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", queues)).Named("MainReceiveProvider").LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", errorQueues)).Named("ErrorsReceiveProvider").LifestyleSingleton(),

                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", responses)).Named("MainQueueProvider").LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", responseNotifications)).Named("NotificationQueueProvider").LifestyleSingleton(),

                Component.For<IEntityMapService>().ImplementedBy<DefaultMapService>().LifestyleSingleton(),

                Component.For<IMessageProvider>().ImplementedBy<DefaultMessageProvider>().LifestyleSingleton()
            );

            container.Register(
                Component.For<ISenderEntity>().ImplementedBy<SenderEntity>().Named("MainSenderEntity")
                    .DependsOn(Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("MainQueueProvider")))
                    .LifestyleSingleton(),
                Component.For<ISenderEntity>().ImplementedBy<SenderEntity>().Named("NotificationSenderEntity")
                    .DependsOn(Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("NotificationQueueProvider")))
                    .LifestyleSingleton()
            );

            container.Register(
                Component.For<Runner>()
                .DependsOn(
                    Dependency.OnValue("queueProvider", container.Resolve<IQueueProvider>("MainReceiveProvider")),
                    Dependency.OnValue("queueProviderErrors", container.Resolve<IQueueProvider>("ErrorsReceiveProvider")),
                    Dependency.OnValue("responseProvider", container.Resolve<ISenderEntity>("MainSenderEntity")),
                    Dependency.OnValue("responseNotifications", container.Resolve<ISenderEntity>("NotificationSenderEntity")),
                    Dependency.OnValue("watchQueue", demandQueue)
                ));

            return container;
        }
    }
}
