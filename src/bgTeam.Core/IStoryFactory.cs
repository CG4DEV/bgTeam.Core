namespace bgTeam
{
    /// <summary>
    /// Интерфейс фабрики, создающей истории для определенного контекста.
    /// </summary>
    public interface IStoryFactory
    {
        /// <summary>
        /// Создает историю по контексту
        /// </summary>
        /// <typeparam name="TStoryContext">Тип контекста</typeparam>
        /// <typeparam name="TResult">Тип возвращаемого значения</typeparam>
        IStory<TStoryContext, TResult> Create<TStoryContext, TResult>();
    }
}
