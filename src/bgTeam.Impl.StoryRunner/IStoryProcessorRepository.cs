namespace bgTeam.Impl.StoryRunner
{
    using bgTeam.Impl.StoryRunner.Domain;

    /// <summary>
    /// Хранилище story
    /// </summary>
    public interface IStoryProcessorRepository
    {
        /// <summary>
        /// Получение story по имени контекста
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        StoryInfo Get(string contextName);
    }
}
