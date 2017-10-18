namespace bgTeam.DataAccess
{
    /// <summary>
    /// Интерфейс для построителя команд.  Позволяет создать и выполнить команду с определенным контекстом.
    /// </summary>
    public interface ICommandBuilder
    {
        /// <summary>
        /// Формирует команду для исполнения
        /// </summary>
        ICommandReturn<TCommandContext> Build<TCommandContext>(TCommandContext commandContext);
    }
}