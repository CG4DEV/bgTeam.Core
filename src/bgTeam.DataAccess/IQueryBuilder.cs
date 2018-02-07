namespace bgTeam.DataAccess
{
    /// <summary>
    /// Интерфейс для построителя команд.  Позволяет создать и выполнить команду с определенным контекстом.
    /// </summary>
    public interface IQueryBuilder
    {
        /// <summary>
        /// Формирует команду для исполнения
        /// </summary>
        IQueryReturn<TCommandContext> Build<TCommandContext>(TCommandContext commandContext);
    }
}