namespace bgTeam.Infrastructure
{
    using bgTeam;
    using Microsoft.Extensions.DependencyInjection;

    public class StoryFactory : IStoryFactory
    {
        private readonly IServiceCollection _services;

        public StoryFactory(IServiceCollection services)
        {
            _services = services;
        }

        public IStory<TStoryContext, TResult> Create<TStoryContext, TResult>()
        {
            var provider = _services.BuildServiceProvider();
            var story = provider.GetService<IStory<TStoryContext, TResult>>();
            return story;
        }
    }
}
