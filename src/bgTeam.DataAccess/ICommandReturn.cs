namespace bgTeam.DataAccess
{
    using System.Threading.Tasks;

    public interface ICommandReturn<TCommandContext>
    {
        /// <summary>
        /// Выполнить команду
        /// </summary>
        void Execute();

        /// <summary>
        /// Выполнить команду
        /// </summary>
        Task ExecuteAsync();

        /// <summary>
        /// Выполнить команду, и вернуть результат
        /// </summary>
        TResult Return<TResult>();

        /// <summary>
        /// Выполнить команду, и вернуть результат
        /// </summary>
        Task<TResult> ReturnAsync<TResult>();
    }
}