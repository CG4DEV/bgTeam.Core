namespace bgTeam.DataAccess
{
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс фабрики, создающей подключение к базе данных
    /// </summary>
    public interface IConnectionFactory
    {
        /// <summary>
        /// Создаёт подключение к базе данных
        /// </summary>
        /// <returns>Объект подключения</returns>
        IDbConnection Create();

        /// <summary>
        /// Создаёт подключение к базе данных по переданной строке подключения
        /// </summary>
        /// <returns>Объект подключения</returns>
        IDbConnection Create(string connectionString);

        /// <summary>
        /// Асинхронно создаёт подключение к базе данных
        /// </summary>
        /// <returns>Объект подключения</returns>
        Task<IDbConnection> CreateAsync();

        /// <summary>
        /// Асинхронно создаёт подключение к базе данных по переданной строке подключения
        /// </summary>
        /// <returns>Объект подключения</returns>
        Task<IDbConnection> CreateAsync(string connectionString);
    }
}
