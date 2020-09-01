namespace bgTeam.Impl.StoryRunner
{
    using System.Threading.Tasks;
    using bgTeam.Queues;

    /// <summary>
    /// Запуск стори
    /// </summary>
    public interface IStoryProcessor
    {
        /// <summary>
        /// Запуск стори из StoryRunnerMessageWork
        /// </summary>
        /// <param name="info"></param>
        void Process(StoryRunnerMessageWork info);

        /// <summary>
        /// Асинхронный запуск стори из StoryRunnerMessageWork
        /// </summary>
        /// <param name="info"></param>
        Task ProcessAsync(StoryRunnerMessageWork info);
    }
}
