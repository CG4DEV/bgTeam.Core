namespace bgTeam
{
    using System.Threading.Tasks;

    /// <summary>
    /// Выполняет историю и возвращает результат.
    /// </summary>
    /// <typeparam name="TStoryContext">Тип контекста</typeparam>
    public interface IStoryReturn<TStoryContext>
    {
        /// <summary>
        /// Выполнить историю асинхронно и вернуть результат
        /// </summary>
        /// <typeparam name="TResult">Тип результата выполнения истории</typeparam>
        Task<TResult> ReturnAsync<TResult>();
    }
}