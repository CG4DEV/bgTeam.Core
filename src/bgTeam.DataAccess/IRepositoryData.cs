namespace bgTeam.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRepositoryData
    {
        IEnumerable<dynamic> GetAll(string sql, object param = null);

        Task<IEnumerable<dynamic>> GetAllAsync(string sql, object param = null);
    }
}
