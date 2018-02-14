namespace bgTeam.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositoryDataTable
    {
        IEnumerable<dynamic> GetAll(string sql, object param = null);

        Task<IEnumerable<dynamic>> GetAllAsync(string sql, object param = null);
    }
}
