namespace bgTeam.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для получения информации из базы данных
    /// </summary>
    public interface IRepositoryDataTable
    {
        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        IEnumerable<dynamic> GetAll(string sql, object param = null);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> GetAllAsync(string sql, object param = null);
    }
}
