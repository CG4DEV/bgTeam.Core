namespace bgTeam.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Интерфейс для получения информации из базы данных
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// Выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        T Get<T>(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        T Get<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос к базе данных на основе предиката и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        T Get<T>(Expression<Func<T, bool>> predicate, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных на основе предиката и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(Expression<Func<T, bool>> predicate, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает объект из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Выполняет запрос к базе данных на основе предиката и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate = null, IList<ISort> sort = null, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="sql">Запрос к базе данных</param>
        /// <param name="param">Параметры запроса</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(string sql, object param = null, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемых объектов</typeparam>
        /// <param name="obj">Объект с запросом к базе данных</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(ISqlObject obj, IDbConnection connection = null, IDbTransaction transaction = null);

        /// <summary>
        /// Асинхронно выполняет запрос к базе данных на основе предиката и возвращает коллекцию объектов из базы данных
        /// </summary>
        /// <typeparam name="T">Тип получаемого объекта</typeparam>
        /// <param name="predicate">Предикат</param>
        /// <param name="connection">Использовать существущее соединение</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate = null, IList<ISort> sort = null, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;

        IEnumerable<T> GetPage<T>(Expression<Func<T, bool>> predicate, IList<ISort> sort, int page, int resultsPerPage)
            where T : class;

        Task<IEnumerable<T>> GetPageAsync<T>(Expression<Func<T, bool>> predicate = null, IList<ISort> sort = null, int page = 0, int resultsPerPage = 10)
            where T : class;

        Task<PaginatedResult<T>> GetPaginatedResultAsync<T>(Expression<Func<T, bool>> predicate = null, IList<ISort> sort = null, int page = 0, int resultsPerPage = 10, IDbConnection connection = null, IDbTransaction transaction = null)
            where T : class;
    }
}
