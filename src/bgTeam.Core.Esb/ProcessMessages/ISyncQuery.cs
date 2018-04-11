namespace bgTeam.ProcessMessages
{
    using System.Threading.Tasks;

    public interface ISyncQuery
    {
        void Execute();

        Task ExecuteAsync();
    }
}
