namespace bgTeam.Web.IoC
{
    using System;
    using System.Net.Http;
    using bgTeam.Web.Impl;
    using Microsoft.Extensions.DependencyInjection;

    public static class IoCExtensions
    {
        private const int DEFAULT_CONNECTIONS_LIMIT = 1024;
        private const int DEFAULT_MAX_IDLE_TIME = 300000; // 5 мин
        private const int DEFAULT_CONNECTION_LEASE_TIMEOUT = 0;

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="baseUrl">Базовый адрес подключения</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(this IServiceCollection services, string baseUrl)
            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            return services.AddWebClient<TInterface, TIMplementation>(baseUrl, DEFAULT_CONNECTIONS_LIMIT, DEFAULT_MAX_IDLE_TIME, DEFAULT_CONNECTION_LEASE_TIMEOUT);
        }

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="baseUrl">Базовый адрес подключения</param>
        /// <param name="connectionsLimit">Количество одновременных запросов на удалённый сервер</param>
        /// <param name="maxIdleMs">Указывает, после какого времени бездействия (в мс) соединение будет закрыто. Бездействие означает отсутствие передачи данных через соединение.</param>
        /// <param name="connectionLeaseTimeoutMs">Указывает, сколько времени (в мс) соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(this IServiceCollection services,
            string baseUrl, int connectionsLimit, int maxIdleMs, int connectionLeaseTimeoutMs)
            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            return services.AddWebClient<TInterface, TIMplementation>(baseUrl, connectionsLimit, TimeSpan.FromMilliseconds(maxIdleMs), TimeSpan.FromMilliseconds(connectionLeaseTimeoutMs));
        }

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="baseUrl">Базовый адрес подключения</param>
        /// <param name="connectionsLimit">Количество одновременных запросов на удалённый сервер</param>
        /// <param name="maxIdle">Указывает, после какого времени бездействия соединение будет закрыто. Бездействие означает отсутствие передачи данных через соединение.</param>
        /// <param name="connectionLeaseTimeout">Указывает, сколько времени соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(this IServiceCollection services,
            string baseUrl, int connectionsLimit, TimeSpan maxIdle, TimeSpan connectionLeaseTimeout)
            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            var clientBuilder = services.AddHttpClient<TInterface, TIMplementation>();

            clientBuilder
                .ConfigureHttpClient(x => x.BaseAddress = new Uri(baseUrl))
                .ConfigurePrimaryHttpMessageHandler(() =>
                {
                    return new SocketsHttpHandler()
                    {
                        MaxConnectionsPerServer = connectionsLimit,
                        PooledConnectionIdleTimeout = maxIdle,
                        PooledConnectionLifetime = connectionLeaseTimeout,
                    };
                });

            return services;
        }

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="baseUrl">Базовый адрес подключения</param>
        /// <param name="configureHandler">создаёт и конфигурирует SocketsHttpHandler</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(this IServiceCollection services, string baseUrl, Func<IServiceProvider, SocketsHttpHandler> configureHandler)
            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            var clientBuilder = services.AddHttpClient<TInterface, TIMplementation>();

            clientBuilder
                .ConfigureHttpClient(x => x.BaseAddress = new Uri(baseUrl))
                .ConfigurePrimaryHttpMessageHandler(configureHandler);

            return services;
        }
    }
}
