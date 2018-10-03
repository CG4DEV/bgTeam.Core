namespace bgTeam.Impl
{
    using System;
    using bgTeam;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Реализация фабрики, создающей истории для определенного контекста.
    /// </summary>
    public class StoryFactory : IStoryFactory
    {
        private readonly IServiceProvider _provider;

        public StoryFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Создает историю по контексту
        /// </summary>
        public IStory<TStoryContext, TResult> Create<TStoryContext, TResult>()
        {
            return _provider.GetService<IStory<TStoryContext, TResult>>();
        }
    }
}
