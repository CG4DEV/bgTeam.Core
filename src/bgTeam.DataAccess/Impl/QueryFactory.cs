namespace bgTeam.DataAccess.Impl
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    public class QueryFactory : IQueryFactory
    {
        private readonly IServiceProvider _provider;

        public QueryFactory(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IQuery<TCommandContext> Create<TCommandContext>()
        {
            return _provider.GetService<IQuery<TCommandContext>>();
        }

        public IQuery<TCommandContext, TResult> Create<TCommandContext, TResult>()
        {
            return _provider.GetService<IQuery<TCommandContext, TResult>>();
        }
    }
}
