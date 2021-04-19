namespace bgTeam.StoryRunner.Impl
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

        private readonly IServiceProvider _container;
        private readonly IStoryProcessorRepository _repository;

        public StoryProcessor(
            IServiceProvider container,
            IStoryProcessorRepository repository)
        {
            _container = container;
            _repository = repository;
        }

        public Task ProcessAsync(StoryRunnerMessageWork info)
        {
            var storyInfo = _repository.Get(info.Name);

            var context = JsonConvert.DeserializeObject(info.Context, storyInfo.ContextType, _settings);
            var story = _container.GetService(storyInfo.StoryType);

            return (Task)storyInfo.ExecuteMethodInfo.Invoke(story, new[] { context });
        }

        public async Task<object> ProcessWithResultAsync(StoryRunnerMessageWork info)
        {
            var task = ProcessAsync(info);
            await task.ConfigureAwait(false);

            var resultProperty = task.GetType().GetProperty("Result");
            return resultProperty.GetValue(task);
        }
    }
}