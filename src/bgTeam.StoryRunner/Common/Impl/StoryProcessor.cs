namespace bgTeam.StoryRunner.Common.Impl
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using Newtonsoft.Json;

    public class StoryProcessor : IStoryProcessor
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        private readonly IAppLogger _logger;
        private readonly IServiceProvider _container;
        private readonly IStoryProcessorRepository _repository;

        public StoryProcessor(
            IAppLogger logger,
            IServiceProvider container,
            IStoryProcessorRepository repository)
        {
            _logger = logger;
            _container = container;
            _repository = repository;
        }

        public void Process(StoryRunnerMessageWork info)
        {
            ProcessAsync(info).Wait();
        }

        public async Task ProcessAsync(StoryRunnerMessageWork info)
        {
            var storyInfo = _repository.Get(info.Name);

            var context = JsonConvert.DeserializeObject(info.Context, storyInfo.ContextType, _settings);
            var story = _container.GetService(storyInfo.StoryType);

            await (Task)storyInfo.ExecuteMethodInfo.Invoke(story, new[] { context });
        }
    }
}