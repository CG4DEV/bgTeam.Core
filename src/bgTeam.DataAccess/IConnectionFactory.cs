namespace bgTeam.DataAccess
{
    using System.Data;
    using System.Threading.Tasks;

    public interface IConnectionFactory
    {
        IDbConnection Create();

        IDbConnection Create(string connectionString);

        Task<IDbConnection> CreateAsync();

        Task<IDbConnection> CreateAsync(string connectionString);
    }
}
