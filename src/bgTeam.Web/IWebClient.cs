namespace bgTeam.Web
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IWebClient
    {
        /// <summary>
        /// Устанавливает количество одновременных соединений с сервером
        /// </summary>
        //int ConnectionsLimit { get; set; }

        /// <summary>
        /// Указывает, сколько времени (в мс) соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.
        /// </summary>
        //int ConnectionLeaseTimeout { get; set; }

        /// <summary>
        /// Указывает, после какого времени бездействия (в мс) соединение будет закрыто. Бездействие означает отсутствие передачи данных через соединение.
        /// </summary>
        //int MaxIdleTime { get; set; }

        /// <summary>
        /// Устанавливает или возвращает таймаут запроса к серверу
        /// </summary>
        TimeSpan RequestTimeout { get; set; }

        /// <summary>
        /// Устанавливает авторизационны хеддер
        /// </summary>
        /// <param name="scheme">схема авторизации</param>
        /// <param name="value">информация о правах доступа</param>
        void SetAuthHeader(string scheme, string value);

        /// <summary>
        /// Выполняет GET-запрос и возвращает результат типа T
        /// </summary>
        /// <typeparam name="T">тип возвращаемого результата</typeparam>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class;

        /// <summary>
        /// Выполняет GET-запрос и проверяет статус ответа от сервера
        /// </summary>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task GetAsync(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null);

        /// <summary>
        /// Выполняет DELETE-запрос и возвращает результат типа T
        /// </summary>
        /// <typeparam name="T">тип возвращаемого результата</typeparam>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task<T> DeleteAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class;

        /// <summary>
        /// Выполняет DELETE-запрос и проверяет статус ответа от сервера
        /// </summary>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task DeleteAsync(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null);

        /// <summary>
        /// Выполняет POST-запрос и возвращает результат типа T
        /// </summary>
        /// <typeparam name="T">тип возвращаемого результата</typeparam>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string method, object postParams = null, IDictionary<string, object> headers = null)
            where T : class;

        /// <summary>
        /// Выполняет POST-запрос и проверяет статус ответа от сервера
        /// </summary>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task PostAsync(string method, object postParams = null, IDictionary<string, object> headers = null);

        /// <summary>
        /// Выполняет PUT-запрос и возвращает результат типа T
        /// </summary>
        /// <typeparam name="T">тип возвращаемого результата</typeparam>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task<T> PutAsync<T>(string method, object putParams = null, IDictionary<string, object> headers = null)
            where T : class;

        /// <summary>
        /// Выполняет PUT-запрос и проверяет статус ответа от сервера
        /// </summary>
        /// <param name="method">вызываемый метод</param>
        /// <param name="queryParams">параметры запроса</param>
        /// <param name="headers">список хеддеров</param>
        /// <returns></returns>
        Task PutAsync(string method, object putParams = null, IDictionary<string, object> headers = null);
    }
}
