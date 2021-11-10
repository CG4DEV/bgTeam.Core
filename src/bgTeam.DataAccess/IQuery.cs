using System.Threading;

namespace bgTeam.DataAccess
{
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для команды.
    /// </summary>
    /// <typeparam name="TCommandContext">Контекст команды</typeparam>
    public interface IQuery<in TCommandContext>
    {
        /// <summary>
        /// Выполняет действия команды.
        /// </summary>
        /// <param name="commandContext">Контекст команды</param>
        void Execute(TCommandContext context);

        Task ExecuteAsync(TCommandContext context, CancellationToken ct = default);
    }

    public interface IQuery<in TCommandContext, TCommandResult>
    {
        /// <summary>
        /// Выполняет действия команды и вернуть результат
        /// </summary>
        /// <param name="commandContext">Контекст команды</param>
        TCommandResult Execute(TCommandContext context);

        Task<TCommandResult> ExecuteAsync(TCommandContext context, CancellationToken ct = default);
    }
}