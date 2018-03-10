namespace bgTeam.Infrastructure
{
    using bgTeam;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Реализация фабрики, создающей истории для определенного контекста.
    /// </summary>
    public class StoryFactory : IStoryFactory
    {
        private readonly IServiceCollection _services;

        public StoryFactory(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Создает историю по контексту
        /// </summary>
        public IStory<TStoryContext, TResult> Create<TStoryContext, TResult>()
        {
            var provider = _services.BuildServiceProvider();

            var story = provider.GetService<IStory<TStoryContext, TResult>>();

            return story;
        }
    }
}
