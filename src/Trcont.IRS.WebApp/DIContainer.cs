namespace Trcont.IRS.WebApp
{
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;
    using Microsoft.Extensions.DependencyInjection;
    using Trcont.IRS.Common;
    using Trcont.IRS.Story;
    using bgTeam.Core;
    using bgTeam.Core.Impl;
    using System.Reflection;
    using Microsoft.AspNetCore.Mvc;
    using bgTeam.Infrastructure;
    using bgTeam.Infrastructure.DataAccess;

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
                    .AddSingleton<IConnectionSetting, AppSettings>()
                    .AddSingleton<IConnectionFactory, ConnectionFactoryMsSql>()
                    .AddTransient<IRepository, RepositoryDapper>()
                    .AddTransient<IRepositoryEntity, RepositoryEntityDapper>()
                    .AddSingleton<ICommandFactory>(commandfactory)
                    .AddSingleton<ICommandBuilder, CommandBuilder>()
                    .AddSingleton<IStoryFactory>(storyfactory)
                    .AddSingleton<IStoryBuilder, StoryBuilder>()
                    .AddSingleton<IMapperBase, AutoMapperStory>()
                    .AddSingleton<IScriptSqlBuilder, ScriptSqlBuilder>()
                    .BuildServiceProvider();
        }
    }
}
