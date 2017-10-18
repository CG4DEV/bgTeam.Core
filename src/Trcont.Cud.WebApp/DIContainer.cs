namespace Trcont.Cud.WebApp
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;
    using Microsoft.Extensions.DependencyInjection;
    using Trcont.Cud.Common;
    using Trcont.Cud.Story;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;
    using bgTeam.Infrastructure;
    using bgTeam.Infrastructure.DataAccess;
    using bgTeam.Web;
    using Trcont.Cud.Common.Impl;
    using Trcont.App.Service.Impl;
    using Trcont.App.Service;
    using Trcont.CitTrans.Service;
    using Trcont.CitTrans.Service.Impl;
    using Trcont.Cud.Infrastructure;
    using Trcont.Cud.Infrastructure.Impl;

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

            services.AddSingleton<IAppConfiguration, AppConfigurationDefault>()
                    .AddSingleton<IAppLoggerConfig, SerilogConfig>()
                    .AddSingleton<IAppLogger, AppLoggerSerilog>()
                    .AddSingleton<ITransPicService, TransPicService>()
                    .AddSingleton<IConnectionSetting, AppSettings>()
                    .AddSingleton<IAppSettings, AppSettings>()
                    .AddSingleton<IStInfoConnectionSetting, StInfoAppSettings>()
                    .AddSingleton<IConnectionFactory, ConnectionFactoryMsSql>()
                    .AddSingleton<IStInfoConnectionFactory, StInfoConnectionFactory>()
                    .AddTransient<IRepository, RepositoryDapper>()
                    .AddTransient<IStInfoRepository, StInfoRepository>()
                    .AddTransient<IRepositoryEntity, RepositoryEntityDapper>()
                    .AddSingleton<ICommandFactory>(commandfactory)
                    .AddSingleton<ICommandBuilder, CommandBuilder>()
                    .AddSingleton<IStoryFactory>(storyfactory)
                    .AddSingleton<IStoryBuilder, StoryBuilder>()
                    .AddSingleton<IMapperBase, AutoMapperStory>()
                    .AddSingleton<IWebClient>(x => {
                        var conf = x.GetService<IAppConfiguration>();
                        return new bgTeam.Web.Impl.WebClient(conf["irsServiceUrl"]);
                    })
                    .AddSingleton<IAddressISales, AddressISales>()
                    .AddSingleton<IAppServiceClient>(x => {
                        var conf = x.GetService<IAppConfiguration>();
                        return new AppServiceClient(conf["appServiceUrl"], x.GetService<IAppLogger>(), x.GetService<IMapperBase>());
                    })
                    .AddSingleton<ICitTransServiceClient>(x => {
                        var conf = x.GetService<IAppConfiguration>();
                        return new CitTransServiceClient(x.GetService<IAppLogger>(), conf["citTransUrl"], x.GetService<IMapperBase>());
                    })
                    .BuildServiceProvider();
        }
    }
}
