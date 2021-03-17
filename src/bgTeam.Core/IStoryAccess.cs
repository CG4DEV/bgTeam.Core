namespace bgTeam
{
    using System.Threading.Tasks;

    /// <summary>
    /// Содержит проверки доступности к историям
    /// </summary>
    public interface IStoryAccess
    {
        /// <summary>
        /// Проверяет доступность стори
        /// </summary>
        /// <typeparam name="TStoryContext">Тип контекста</typeparam>
        /// <typeparam name="TResult">Тип результат</typeparam>
        /// <param name="story">Стори для проверки доступности</param>
        /// <returns></returns>
        Task CheckAccessAsync<TStoryContext, TResult>(IStory<TStoryContext, TResult> story);
    }
}
