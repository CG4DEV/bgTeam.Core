namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    internal static class AppIocConfigure
    {
        public static IServiceProvider Configure(Dictionary<string, string> cmdParams)
        {
            var services = new ServiceCollection();

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            services
                .AddSingleton<IServiceCollection>(services)
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<IAppSettings, AppSettings>()
                .AddTransient<Runner>();

            return services.BuildServiceProvider();
        }
    }
}
