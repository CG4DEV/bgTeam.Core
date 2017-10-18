namespace bgTeam.DataAccess
{
    /// <summary>
    /// Интерфейс фабрики, создающей команду для определенного контекста.
    /// </summary>
    public interface ICommandFactory
    {
        /// <summary>
        /// Создает команду по контексту команды.
        /// </summary>
        ICommand<TCommandContext> Create<TCommandContext>();

        /// <summary>
        /// Создает команду по контексту команды.
        /// </summary>
        ICommand<TCommandContext, TResult> Create<TCommandContext, TResult>();
    }
}