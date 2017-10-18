namespace bgTeam
{
    using System.Threading.Tasks;

    public interface IStoryReturn<TStoryContext>
    {
        /// <summary>
        /// Выполнить историю, и вернуть результат
        /// </summary>
        TResult Return<TResult>();

        /// <summary>
        /// Выполнить историю, и вернуть результат
        /// </summary>
        Task<TResult> ReturnAsync<TResult>();
    }
}