namespace bgTeam.ProcessMessages
{
    using System.Data;
    using System.Threading.Tasks;

    public interface ISyncQuery
    {
        void Execute(IDbConnection connection = null, IDbTransaction transaction = null);

        Task ExecuteAsync(IDbConnection connection = null, IDbTransaction transaction = null);
    }
}
