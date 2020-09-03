namespace bgTeam.Impl.StoryRunner.Impl
{
    using System;
    using System.Threading.Tasks;
    using bgTeam.Queues;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;

    /// <summary>
    /// Реализация IStoryProcessor
    /// </summary>
    public class StoryProcessor : IStoryProcessor
    {
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
        };

        private readonly IAppLogger _logger;
        private readonly IServiceProvider _container;
        private readonly IStoryProcessorRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StoryProcessor"/> class.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="container"></param>
        /// <param name="repository"></param>
        public StoryProcessor(
            IAppLogger logger,
            IServiceProvider container,
            IStoryProcessorRepository repository)
        {
            _logger = logger;
            _container = container;
            _repository = repository;
        }

        /// <summary>
        /// Запуск стори из StoryRunnerMessageWork
        /// </summary>
        /// <param name="info"></param>
        public void Process(StoryRunnerMessageWork info)
        {
            ProcessAsync(info).Wait();
        }

        /// <summary>
        /// Асинхронный запуск стори из StoryRunnerMessageWork
        /// </summary>
        /// <param name="info"></param>
        public async Task ProcessAsync(StoryRunnerMessageWork info)
        {
            var storyInfo = _repository.Get(info.Name);
            var context = JsonConvert.DeserializeObject(info.Context, storyInfo.ContextType, _settings);
            using (var scope = _container.CreateScope())
            {
                var story = scope.ServiceProvider.GetService(storyInfo.StoryType);
                await (Task)storyInfo.ExecuteMethodInfo.Invoke(story, new[] { context });
            }
        }
    }
}