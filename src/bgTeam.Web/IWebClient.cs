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
        int ConnectionsLimit { get; set; }

        /// <summary>
        /// Указывает, сколько времени (в мс) будет закеширован полученный IP адрес для каждого доменного имени
        /// </summary>
        int DnsRefreshTimeout { get; set; }

        /// <summary>
        /// Указывает, сколько времени (в мс) соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.
        /// </summary>
        int ConnectionLeaseTimeout { get; set; }

        /// <summary>
        /// Указывает, после какого времени бездействия (в мс) соединение будет закрыто. Бездействие означает отсутствие передачи данных через соединение.
        /// </summary>
        int MaxIdleTime { get; set; }

        /// <summary>
        /// Устанавливает или возвращает таймаут запроса к серверу
        /// </summary>
        TimeSpan RequestTimeout { get; set; }

        Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class;

        Task<T> PostAsync<T>(string method, object postParams = null, IDictionary<string, object> headers = null)
            where T : class;
    }
}
