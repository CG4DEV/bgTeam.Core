namespace bgTeam.Infrastructure
{
    using bgTeam.DataAccess;
    using Microsoft.Extensions.DependencyInjection;

    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceCollection _services;

        public QueryFactory(IServiceCollection services)
        {
            _services = services;
        }

        public IQuery<TCommandContext> Create<TCommandContext>()
        {
            var provider = _services.BuildServiceProvider();
            return provider.GetService<IQuery<TCommandContext>>();
        }

        public IQuery<TCommandContext, TResult> Create<TCommandContext, TResult>()
        {
            var provider = _services.BuildServiceProvider();
            return provider.GetService<IQuery<TCommandContext, TResult>>();
        }
    }
}
