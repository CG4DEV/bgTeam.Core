namespace Trcont.OrderRoutesCreator
{
    using System;
    using bgTeam;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using bgTeam.DataAccess;
    using bgTeam.Infrastructure;
    using bgTeam.Infrastructure.DataAccess;
    using Castle.Facilities.TypedFactory;
    using Castle.MicroKernel.Registration;
    using Castle.Windsor;
    using global::Quartz;
    using global::Quartz.Impl;
    using Quartz;
    using Quartz.Impl;
    using Trcont.RIS.Common;
    using Trcont.RIS.Common.Impl;

    class Program
    {
        static void Main(string[] args)
        {
            var container = WindsorConfigure();

            var runner = container.Resolve<Runner>();
            runner.Run();

            Console.WriteLine("Press any key to close the application");
            Console.ReadKey();

            runner.Stop();
        }

        public static WindsorContainer WindsorConfigure()
        {
            var container = new WindsorContainer();
            container.AddFacility<TypedFactoryFacility>();

            container.Register(
                Component.For<IAppConfiguration>().ImplementedBy<AppConfigurationDefault>().LifestyleSingleton(),
                Component.For<IAppLoggerConfig>().ImplementedBy<SerilogConfig>().LifestyleSingleton(),
                Component.For<IAppLogger>().ImplementedBy<AppLoggerSerilog>().LifestyleSingleton(),
                Component.For<IConnectionSetting, IApplicationSetting>().ImplementedBy<AppSettings>().LifestyleSingleton(),
                Component.For<IConnectionFactory>().ImplementedBy<ConnectionFactoryMsSql>().LifestyleSingleton(),
                Component.For<IRepository>().ImplementedBy<RepositoryDapper>().LifestyleSingleton(),
                Component.For<ISchedulerFactory>().ImplementedBy<StdSchedulerFactory>().LifestyleSingleton(),
                Component.For<IOrderRoutesCreatorService>().ImplementedBy<OrderRoutesCreatorService>().LifestyleSingleton()
            );

            container.Register(
                Component.For<ISchedulersFactory>().ImplementedBy<SchedulersFactory>().LifestyleSingleton(),
                Component.For<Runner>()
            );

            return container;
        }
    }
}
