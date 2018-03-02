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
        IStory<TStoryContext, TResult> Create<TStoryContext, TResult>();
            //where TCommandContext : ICommandContext;
    }
}
