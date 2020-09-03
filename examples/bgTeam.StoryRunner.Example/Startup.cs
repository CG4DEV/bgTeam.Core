using bgTeam.Core;
using bgTeam.Core.Impl;
using bgTeam.Impl.Rabbit;
using bgTeam.Impl.Serilog;
using bgTeam.Queues;
using bgTeam.StoryRunner.Common;
using bgTeam.StoryRunner.Common.Impl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bgTeam.StoryRunner.Example
{
    internal class Startup : StoryRunner.Startup
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
            var queue = config["AppStoryQueue"];
            var threads = int.Parse(config["AppThreadCount"]);

            services
                .AddSingleton<IAppConfiguration>(config)
                .AddSingleton<IAppLogger, AppLoggerSerilog>(s =>
                {
                    var conf = new AppLoggerSerilogConfig();

                    return new AppLoggerSerilog(conf);
                })
                .AddSingleton<IMessageProvider, MessageProviderDefault>()
                .AddSingleton<IQueueProviderSettings>(appSettings)
                .AddSingleton<IQueueProvider>(s =>
                {
                    var logger = s.GetService<IAppLogger>();
                    var mp = s.GetService<IMessageProvider>();
                    var qps = s.GetService<IQueueProviderSettings>();
                    var cs = new ConnectionFactoryRabbitMQ(logger, qps);

                    return new QueueProviderRabbitMQ(logger, mp, cs, true, queue);
                })
                .AddSingleton<IQueueWatcher<IQueueMessage>>(s =>
                {
                    var logger = s.GetService<IAppLogger>();
                    var mp = s.GetService<IMessageProvider>();
                    var qps = s.GetService<IQueueProviderSettings>();
                    var cs = new ConnectionFactoryRabbitMQ(logger, qps);

                    //return new QueueWatcherRabbitMQ(logger, mp, cs, threads);
                    return new QueueConsumerAsyncRabbitMQ(cs, mp);
                })
                .AddSingleton<IStoryProcessorRepository, StoryProcessorRepository>()
                .AddSingleton<IStoryProcessor, StoryProcessor>();

            // подключение к БД
            //services
            //    .AddTransient<IRepository, Repository>();

            services
                .AddSingleton<Runner>();

            // Регистрация Story
            //services.Scan(scan => scan
            //         .FromAssemblyOf<IStoryLibrary>()
            //         .AddClasses(classes => classes.AssignableTo(typeof(IStory<,>)))
            //         .AsImplementedInterfaces()
            //         .WithTransientLifetime());
        }
    }
}
