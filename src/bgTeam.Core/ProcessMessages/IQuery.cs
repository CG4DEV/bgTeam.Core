namespace bgTeam.ProcessMessages
{
    using System.Threading.Tasks;

    public interface IQuery
    {
        void Execute();

        Task ExecuteAsync();
    }
}
