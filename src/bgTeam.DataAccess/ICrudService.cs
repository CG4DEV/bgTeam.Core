namespace bgTeam.DataAccess
{
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для выполнения запросов на изменение записей в базе данных
    /// </summary>
    public interface ICrudService
    {
        /// <summary>
        /// Вставляет объект типа T в базу данных
        /// </summary>
        /// <typeparam name="T">Тип вставляемого объекта</typeparam>
        /// <param name="entity">Объект для вставки</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        bool Insert<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно вставляет объект типа T в базу данных
        /// </summary>
        /// <typeparam name="T">Тип вставляемого объекта</typeparam>
        /// <param name="entity">Объект для вставки</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        Task<bool> InsertAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Обновляет запись базы данных на основе информации из объекта типа T
        /// </summary>
        /// <typeparam name="T">Тип обновляемого объекта</typeparam>
        /// <param name="entity">Объект для обновления</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        bool Update<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно обновляет запись базы данных на основе информации из объекта типа T
        /// </summary>
        /// <typeparam name="T">Тип обновляемого объекта</typeparam>
        /// <param name="entity">Объект для обновления</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        Task<bool> UpdateAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Удаляет запись базы данных на основе информации из объекта типа T
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="entity">Объект для удаляемого</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        bool Delete<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно удаляет запись базы данных на основе информации из объекта типа T
        /// </summary>
        /// <typeparam name="T">Тип удаляемого объекта</typeparam>
        /// <param name="entity">Объект для удаляемого</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns>True - при удачной вставке</returns>
        Task<bool> DeleteAcync<T>(T entity, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Выполняет запрос, хранящийся в объекте obj
        /// </summary>
        /// <param name="obj">Объект с запросом</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns></returns>
        int Execute(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Асинхронно выполняет запрос, хранящийся в объекте obj
        /// </summary>
        /// <param name="obj">Объект с запросом</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос msg
        /// </summary>
        /// <param name="obj">Объект с запросом</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns></returns>
        int Execute(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Асинхронно выполняет запрос msg
        /// </summary>
        /// <param name="obj">Объект с запросом</param>
        /// <param name="connection">Подключение к базе данных</param>
        /// <param name="transaction">Открытая транзакция</param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);
    }
}
