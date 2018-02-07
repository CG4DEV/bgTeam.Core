namespace bgTeam.DataAccess
{
    /// <summary>
    /// Интерфейс фабрики, создающей команду для определенного контекста.
    /// </summary>
    public interface IQueryFactory
    {
        /// <summary>
        /// Создает команду по контексту команды.
        /// </summary>
        IQuery<TCommandContext> Create<TCommandContext>();

        /// <summary>
        /// Создает команду по контексту команды.
        /// </summary>
        IQuery<TCommandContext, TResult> Create<TCommandContext, TResult>();
    }
}