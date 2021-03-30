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
        /// <param name="name">именует клиент и создаёт для него уникальные настройки</param>
        /// <param name="handlerLifeTime">Время в течении, которого может HttpMessageHandler переиспользоваться. По-умолчанию 2 мин</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(
            this IServiceCollection services,
            string baseUrl,
            string name = null,
            TimeSpan? handlerLifeTime = null)

            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            return services.AddWebClient<TInterface, TIMplementation>(
                baseUrl,
                DEFAULT_CONNECTIONS_LIMIT,
                DEFAULT_MAX_IDLE_TIME,
                DEFAULT_CONNECTION_LEASE_TIMEOUT,
                name,
                handlerLifeTime);
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
        /// <param name="name">именует клиент и создаёт для него уникальные настройки</param>
        /// <param name="handlerLifeTime">Время в течении, которого может HttpMessageHandler переиспользоваться. По-умолчанию 2 мин</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(
            this IServiceCollection services,
            string baseUrl,
            int connectionsLimit,
            int maxIdleMs,
            int connectionLeaseTimeoutMs,
            string name = null,
            TimeSpan? handlerLifeTime = null)

            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            return services.AddWebClient<TInterface, TIMplementation>(
                baseUrl,
                connectionsLimit,
                TimeSpan.FromMilliseconds(maxIdleMs),
                TimeSpan.FromMilliseconds(connectionLeaseTimeoutMs),
                name,
                handlerLifeTime);
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
        /// <param name="name">именует клиент и создаёт для него уникальные настройки</param>
        /// <param name="handlerLifeTime">Время в течении, которого может HttpMessageHandler переиспользоваться. По-умолчанию 2 мин</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(
            this IServiceCollection services,
            string baseUrl,
            int connectionsLimit,
            TimeSpan maxIdle,
            TimeSpan connectionLeaseTimeout,
            string name = null,
            TimeSpan? handlerLifeTime = null)

            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            Action<IServiceProvider, HttpClient> configureClient = (sp, c) => c.BaseAddress = new Uri(baseUrl);
            Func<IServiceProvider, SocketsHttpHandler> configureHandler = sp => new SocketsHttpHandler()
            {
                MaxConnectionsPerServer = connectionsLimit,
                PooledConnectionIdleTimeout = maxIdle,
                PooledConnectionLifetime = connectionLeaseTimeout,
            };

            return services.AddWebClient<TInterface, TIMplementation>(configureClient, configureHandler, name, handlerLifeTime);
        }

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="baseUrl">Базовый адрес подключения</param>
        /// <param name="configureHandler">создаёт и конфигурирует SocketsHttpHandler</param>
        /// <param name="name">именует клиент и создаёт для него уникальные настройки</param>
        /// <param name="handlerLifeTime">Время в течении, которого может HttpMessageHandler переиспользоваться. По-умолчанию 2 мин</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(
            this IServiceCollection services,
            string baseUrl,
            Func<IServiceProvider, SocketsHttpHandler> configureHandler,
            string name = null,
            TimeSpan? handlerLifeTime = null)

            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            return services.AddWebClient<TInterface, TIMplementation>((sp, c) => c.BaseAddress = new Uri(baseUrl), configureHandler, name, handlerLifeTime);
        }

        /// <summary>
        /// Регистрирует новый типизированный HttpClient
        /// </summary>
        /// <typeparam name="TInterface">Интерфейс клиента</typeparam>
        /// <typeparam name="TIMplementation">Реализация клиента</typeparam>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="configureClient">конфигурирует клиент</param>
        /// <param name="configureHandler">создаёт и конфигурирует SocketsHttpHandler</param>
        /// <param name="name">именует клиент и создаёт для него уникальные настройки</param>
        /// <param name="handlerLifeTime">Время в течении, которого может HttpMessageHandler переиспользоваться. По-умолчанию 2 мин</param>
        /// <returns></returns>
        public static IServiceCollection AddWebClient<TInterface, TIMplementation>(
            this IServiceCollection services,
            Action<IServiceProvider, HttpClient> configureClient,
            Func<IServiceProvider, SocketsHttpHandler> configureHandler,
            string name = null,
            TimeSpan? handlerLifeTime = null)

            where TInterface : class
            where TIMplementation : WebClient, TInterface
        {
            var clientBuilder = string.IsNullOrWhiteSpace(name)
                ? services.AddHttpClient<TInterface, TIMplementation>()
                : services.AddHttpClient<TInterface, TIMplementation>(name);

            clientBuilder
                .ConfigureHttpClient(configureClient)
                .ConfigurePrimaryHttpMessageHandler(configureHandler);

            if (handlerLifeTime.HasValue)
            {
                clientBuilder.SetHandlerLifetime(handlerLifeTime.Value);
            }

            return services;
        }
    }
}
