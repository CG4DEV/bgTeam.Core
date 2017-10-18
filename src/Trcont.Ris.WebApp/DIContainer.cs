namespace Trcont.Ris.WebApp
{
    using bgTeam;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;
    using bgTeam.Infrastructure;
    using bgTeam.Infrastructure.DataAccess;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    using Trcont.App.Service;
    using Trcont.App.Service.Impl;
    using Trcont.Ris.Common;
    using Trcont.Ris.Common.Impl;
    using Trcont.Ris.Story;
    using Trcont.CitTrans.Service;
    using Trcont.CitTrans.Service.Impl;
    using bgTeam.Web;
    using bgTeam.Web.Impl;

    public static class DIContainer
    {
        public static void Configure(IServiceCollection services)
        {
            services.Scan(scan => scan
                    .FromAssemblyOf<IStoryLibrary>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IStory<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            services.Scan(scan => scan
                    .FromAssemblies(Assembly.GetExecutingAssembly())
                    .AddClasses(classes => classes.AssignableTo<Controller>())
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());

            var commandfactory = new CommandFactory(services);
            var storyfactory = new StoryFactory(services);

            var config = new AppConfigurationDefault();
            var irsUrl = config["irsServiceUrl"];
            var appUrl = config["appServiceUrl"];
            var citTransUrl = config["citTransUrl"];

            services.AddSingleton<IAppConfiguration>(config)
                .AddSingleton<IAppLoggerConfig, SerilogConfig>()
                .AddSingleton<IAppLogger, AppLoggerSerilog>()
                .AddSingleton<IWebClient>(new WebClient(irsUrl))
                .AddSingleton<IConnectionSetting, AppSettings>()
                .AddSingleton<IConnectionFactory, ConnectionFactoryMsSql>()
                .AddTransient<IRepository, RepositoryDapper>()
                .AddTransient<IRepositoryEntity, RepositoryEntityDapper>()
                .AddSingleton<ITransPicService, TransPicService>()
                .AddSingleton<ICommandFactory>(commandfactory)
                .AddSingleton<ICommandBuilder, CommandBuilder>()
                .AddSingleton<IStoryFactory>(storyfactory)
                .AddSingleton<IStoryBuilder, StoryBuilder>()
                .AddSingleton<IMapperBase, AutoMapperStory>()
                .AddSingleton<IOrderServiceService, OrderServiceService>()
                .AddSingleton<IAppServiceClient, AppServiceClient>(serviceProvider =>
                {
                    var logger = serviceProvider.GetService<IAppLogger>();
                    var mapper = serviceProvider.GetService<IMapperBase>();
                    return new AppServiceClient(appUrl, logger, mapper);
                })
                .AddSingleton<ICitTransServiceClient, CitTransServiceClient>(serviceProvider =>
                {
                    var logger = serviceProvider.GetService<IAppLogger>();
                    var mapper = serviceProvider.GetService<IMapperBase>();
                    return new CitTransServiceClient(logger, citTransUrl, mapper);
                })
                .AddSingleton<IFssToIrsConvertor, FssToIrsConvertor>()
                .BuildServiceProvider();
        }
    }
}
