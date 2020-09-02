namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
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

            // TODO: внести исправления в базовую сборку класс AppConfigurationDefault
            if (cmdParams.ContainsKey("env"))
            {
                // Задаём конфигурацию через пришедший параметр
                config = new AppConfigurationDefault(cmdParams["env"]);
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
                .AddTransient<Runner>();

            return services.BuildServiceProvider();
        }
    }
}
