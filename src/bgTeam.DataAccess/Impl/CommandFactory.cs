namespace bgTeam.Infrastructure
{
    using bgTeam.DataAccess;
    using Microsoft.Extensions.DependencyInjection;

    public class CommandFactory : ICommandFactory
    {
        private readonly IServiceCollection _services;

        public CommandFactory(IServiceCollection services)
        {
            _services = services;
        }

        public ICommand<TCommandContext> Create<TCommandContext>()
        {
            var provider = _services.BuildServiceProvider();
            return provider.GetService<ICommand<TCommandContext>>();
        }

        public ICommand<TCommandContext, TResult> Create<TCommandContext, TResult>()
        {
            var provider = _services.BuildServiceProvider();
            return provider.GetService<ICommand<TCommandContext, TResult>>();
        }
    }
}
