using System.Threading;

namespace bgTeam.DataAccess
{
    using System.Threading.Tasks;

    public interface IQueryReturn<TCommandContext>
    {
        /// <summary>
        /// Выполнить команду
        /// </summary>
        void Execute();

        /// <summary>
        /// Выполнить команду
        /// </summary>
        Task ExecuteAsync(CancellationToken ct = default);

        /// <summary>
        /// Выполнить команду, и вернуть результат
        /// </summary>
        TResult Return<TResult>();

        /// <summary>
        /// Выполнить команду, и вернуть результат
        /// </summary>
        Task<TResult> ReturnAsync<TResult>(CancellationToken ct = default);
    }
}