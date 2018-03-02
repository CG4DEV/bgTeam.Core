namespace bgTeam
{
    using System.Threading.Tasks;

    /// <summary>
    /// Интевфейс истории
    /// </summary>
    /// <typeparam name="TStoryContext">Тип контекста</typeparam>
    /// <typeparam name="TStoryResult">Тип возвращаемого значения</typeparam>
    public interface IStory<in TStoryContext, TStoryResult>
        ////where TStoryContext : IStoryContext
    {
        /// <summary>
        /// Выполняет действия команды и возвращает результат
        /// </summary>
        /// <param name="commandContext">Контекст команды</param>
        TStoryResult Execute(TStoryContext context);

        /// <summary>
        /// Асинхронно выполняет действия команды и возвращает результат
        /// </summary>
        /// <param name="commandContext">Контекст команды</param>
        Task<TStoryResult> ExecuteAsync(TStoryContext context);
    }
}
