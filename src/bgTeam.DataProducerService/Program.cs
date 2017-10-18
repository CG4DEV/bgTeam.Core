namespace bgTeam.DataProducerService
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
    using bgTeam.DataProducerService.NCron;
    using bgTeam.DataProducerService.NCron.Impl;
    using bgTeam.Extensions;
    using bgTeam.Infrastructure;
    using bgTeam.Plugins;
    using bgTeam.ProduceMessages;
    using bgTeam.Queues;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using Quartz;
    using Quartz.Impl;
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

            var runner = container.Resolve<Runner>();
            runner.Run();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();

            runner.Stop();
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

            var queues = new[] { appConf["ProducerQueue"] };
            var notifiationQueues = queues.Select(x => x + ".Notification").ToArray();

            container.Register(
                Component.For<IAppLoggerConfig>().ImplementedBy<SerilogConfig>().LifestyleSingleton(),
                Component.For<IAppLogger>().ImplementedBy<AppLoggerSerilog>().LifestyleSingleton(),
                Component.For<IConnectionSetting, IQueueProviderSettings>().ImplementedBy<AppSettings>().LifestyleSingleton(),


                Component.For<IConnectionFactory>().ImplementedBy<ConnectionFactoryMsSql>().LifestyleSingleton(),
                Component.For<IRepositoryData>().ImplementedBy<RepositoryData>().LifestyleSingleton(),
                Component.For<IRepository>().ImplementedBy<RepositoryDapper>().LifestyleSingleton(),
                Component.For<IRepositoryEntity>().ImplementedBy<RepositoryEntityDapper>().LifestyleSingleton(),

                Component.For<ISchedulerFactory>().ImplementedBy<StdSchedulerFactory>().LifestyleSingleton(),

                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", queues)).Named("MainQueueProvider").LifestyleSingleton(),
                Component.For<IQueueProvider>().ImplementedBy<QueueProviderRabbitMQ>()
                    .DependsOn(Dependency.OnValue("queues", notifiationQueues)).Named("NotificationQueueProvider").LifestyleSingleton(),

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

            LoadPlugins(container);

            IEnumerable<IPluginSchedulersFactory> plugins = 
                container.Kernel.HasComponent(typeof(IPluginSchedulersFactory)) ?
                container.ResolveAll<IPluginSchedulersFactory>() :
                Enumerable.Empty<IPluginSchedulersFactory>();

            container.Register(
                Component.For<ISchedulersFactory>().ImplementedBy<SchedulersFactory>()
                    .DependsOn(Dependency.OnValue("sender", container.Resolve<ISenderEntity>("MainSenderEntity")))
                    .DependsOn(Dependency.OnValue("notificationSender", container.Resolve<ISenderEntity>("NotificationSenderEntity")))
                    .LifestyleSingleton(),

                Component.For<Runner>()
                    .DependsOn(Dependency.OnValue("pluginSchedulersFactories", plugins))
            );

            return container;
        }

        private static void LoadPlugins(WindsorContainer container)
        {
            var fileInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);
            var pluginsFolderPath = Path.Combine(fileInfo.Directory.FullName, "plugins");

            var types = PluginLoader.Load<IPluginSchedulersFactory>(pluginsFolderPath);

            types.DoForEach(x => container.Register(
                Component.For<IPluginSchedulersFactory>().ImplementedBy(x).NamedAutomatically(x.FullName)
                .DependsOn(Dependency.OnValue("sender", container.Resolve<ISenderEntity>("MainSenderEntity")))
                .DependsOn(Dependency.OnValue("notificationSender", container.Resolve<ISenderEntity>("NotificationSenderEntity")))
                .LifestyleSingleton()));
        }
    }
}
