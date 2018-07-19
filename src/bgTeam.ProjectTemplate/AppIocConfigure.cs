namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using bgTeam.Impl;
    using Microsoft.Extensions.DependencyInjection;

    internal static class AppIocConfigure
    {
        public static IServiceProvider Configure(Dictionary<string, string> cmdParams)
        {
            var services = new ServiceCollection();

            IAppConfiguration config;
            //TODO внести исправления в базовую сборку класс AppConfigurationDefault
            if (cmdParams.ContainsKey("env"))
            {
                // Задаём конфигурацию через пришедший параметр
                config = new AppConfigurationDefault("appsettings", cmdParams["env"]);
            }
            else
            {
                // Задаём конфигурацию через переменную среды
                config = new AppConfigurationDefault();
            }

            var appSettings = new AppSettings(config);

            services
                .AddSingleton<IServiceCollection>(services)
                .AddSingleton<IAppConfiguration>(config)
                .AddSingleton<IAppSettings>(appSettings)
                .AddSingleton<IAppLogger, AppLoggerDefault>()

                //.AddSingleton<IStoryFactory, StoryFactory>()
                //.AddSingleton<IStoryBuilder, StoryBuilder>()
                //.AddSingleton<IConnectionSetting>(appSettings)
                //.AddSingleton<IFtpSettings>(appSettings)
                //.AddSingleton<IConnectionFactory, ConnectionFactoryMsSql>()
                //.AddSingleton<IRepository, RepositoryDapper>()
                //.AddSingleton<ICrudService, CrudServiceDapper>()
                //.AddSingleton<IFileLoader, FtpLoader>()
                //.AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
                //.AddSingleton<ISchedulersFactory, SchedulersFactory>()
                .AddTransient<Runner>();

            return services.BuildServiceProvider();
        }
    }
}
