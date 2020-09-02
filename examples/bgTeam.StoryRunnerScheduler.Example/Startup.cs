using bgTeam.Core;
using bgTeam.Core.Impl;
using bgTeam.Impl.Rabbit;
using bgTeam.Impl.Serilog;
using bgTeam.Quartz;
using bgTeam.Queues;
using bgTeam.Queues.Impl;
using bgTeam.StoryRunnerScheduler.Scheduler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;

namespace bgTeam.StoryRunnerScheduler.Example
{
    internal class Startup : bgTeam.StoryRunnerScheduler.Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration config) : base(env, config)
        {
        }

        public override void ConfigureServiceCollection(IServiceCollection services)
        {
#if !DEBUG
            var config = new AppConfigurationDefault("appsettings", "Production");
#else
            var config = new AppConfigurationDefault("appsettings", "Development");
#endif
            var appSettings = new AppSettings(config);

            services
                .AddSingleton<IAppConfiguration>(config)
                .AddSingleton<IAppLogger, AppLoggerSerilog>()
                .AddSingleton<ISchedulerFactory, StdSchedulerFactory>()
                .AddSingleton<ISchedulersFactory, JobSchedulersFactory>()

                .AddSingleton<IMessageProvider, MessageProviderDefault>()
                .AddSingleton<IQueueProviderSettings>(appSettings)
                .AddSingleton<IQueueProvider, QueueProviderRabbitMQ>()
                .AddSingleton<ISenderEntity, SenderEntityDefault>()

                // регистрациия работы с бд
                //.AddSingleton<IConnectionSetting>(appSettings)
                //.AddSingleton<ISqlDialect, SqlDialectDapper>()
                //.AddSingleton<IConnectionFactory, ConnectionFactoryPostgreSQL>()
                //.AddSingleton<IRepository, RepositoryDapper>()
                ;

            services
                .AddSingleton<Runner>();
        }
    }
}
