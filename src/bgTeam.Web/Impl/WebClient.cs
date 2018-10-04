namespace bgTeam.Web.Impl
{
    using bgTeam.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
#if !(NETCOREAPP2_1 || NETCOREAPP2_0)
    using System.Net;
#endif
    using System.Net.Http;
    using System.Threading.Tasks;

    public class WebClient : IWebClient
    {
        private readonly string _url;
        private readonly IAppLogger _logger;
        private readonly HttpClient _client;

#if NETCOREAPP2_1
        private readonly SocketsHttpHandler _handler;
#elif NETCOREAPP2_0
        private readonly HttpClientHandler _handler;
#else
        private readonly ServicePoint _servicePoint;
#endif

        public WebClient(IAppLogger logger, string url)
        {
            _logger = logger;
            _url = url.CheckNull(nameof(url));

            var uri = new Uri(_url);
#if NETCOREAPP2_1
            _handler = new SocketsHttpHandler();
            _client = new HttpClient(_handler);
#elif NETCOREAPP2_0
             _handler = new HttpClientHandler();
             _client = new HttpClient(_handler);
#else
            _client = new HttpClient();
            _servicePoint = ServicePointManager.FindServicePoint(uri);
#endif

            _client.BaseAddress = uri;
        }

        /// <summary>
        /// Количество одновременных запросов на удалённый сервер. По-умолчанию для .net core int.Max, в остальных случаях 2
        /// </summary>
        public int ConnectionsLimit
        {
            get
            {
#if NETCOREAPP2_1 || NETCOREAPP2_0
                return _handler.MaxConnectionsPerServer;
#else
                return ServicePointManager.DefaultConnectionLimit;
#endif
            }

            set
            {
#if NETCOREAPP2_1 || NETCOREAPP2_0
                _handler.MaxConnectionsPerServer = value;
#else
                ServicePointManager.DefaultConnectionLimit = value;
#endif
            }
        }

        public string Test
        {
            get
            {
#if NETCOREAPP2_1
                return "Core 2.1";
#elif NETCOREAPP2_0
                return "Core 2.0";
#else
                return "Standart";
#endif
            }
        }

        /// <summary>
        /// Указывает, сколько времени (в мс) будет закеширован полученный IP адрес для каждого доменного имени
        /// </summary>
        /// <exception>
        /// NotSupportedException для сред .net core
        /// </exception>
        public int DnsRefreshTimeout
        {
            get
            {
#if NETCOREAPP2_1 || NETCOREAPP2_0
                throw new NotSupportedException();
#else
                return ServicePointManager.DnsRefreshTimeout;
#endif
            }

            set
            {
#if NETCOREAPP2_1 || NETCOREAPP2_0
                throw new NotSupportedException();
#else
                ServicePointManager.DnsRefreshTimeout = value;
#endif
            }
        }

        /// <summary>
        /// Указывает, после какого времени бездействия (в мс) соединение будет закрыто. Бездействие означает отсутствие передачи данных через соединение.
        /// </summary>
        /// <exception>
        /// NotSupportedException для среды .net core 2.0
        /// </exception>
        public int MaxIdleTime
        {
            get
            {
#if NETCOREAPP2_1
                return Convert.ToInt32(_handler.PooledConnectionIdleTimeout.TotalMilliseconds);
#elif NETCOREAPP2_0
                throw new NotSupportedException();
#else
                return _servicePoint.MaxIdleTime;
#endif
            }

            set
            {
#if NETCOREAPP2_1
                _handler.PooledConnectionIdleTimeout = TimeSpan.FromSeconds(value);
#elif NETCOREAPP2_0
                throw new NotSupportedException();
#else
                _servicePoint.MaxIdleTime = value;
#endif
            }
        }

        /// <summary>
        /// Указывает, сколько времени (в мс) соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.
        /// </summary>
        /// <exception>
        /// NotSupportedException для среды .net core 2.0
        /// </exception>
        public int ConnectionLeaseTimeout
        {
            get
            {
#if NETCOREAPP2_1
                return Convert.ToInt32(_handler.PooledConnectionLifetime.TotalMilliseconds);
#elif NETCOREAPP2_0
                throw new NotSupportedException();
#else
                return _servicePoint.ConnectionLeaseTimeout;
#endif
            }

            set
            {
#if NETCOREAPP2_1
                _handler.PooledConnectionLifetime = TimeSpan.FromSeconds(value);
#elif NETCOREAPP2_0
                throw new NotSupportedException();
#else
                _servicePoint.ConnectionLeaseTimeout = value;
#endif
            }
        }

        public string Url => _url;

        public async Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            if (string.IsNullOrWhiteSpace(_url))
            {
                method = method.CheckNull(nameof(method));
            }

            string url = BuildGetUrl(method, queryParams);
            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            FillHeaders(headers, msg);

            var resultGet = await _client.SendAsync(msg);
            return await ProcessResult<T>(resultGet);
        }

        public async Task<T> PostAsync<T>(string action, object postParams = null)
            where T : class
        {
            var result = string.Empty;

            if (postParams != null)
            {
                var dic = GetFormContentDictionary(postParams);
                var content = new FormUrlEncodedContent(dic);
                var resultPost = await _client.PostAsync(action, content);
                result = await resultPost.Content.ReadAsStringAsync();
            }
            else
            {
                var resultPost = await _client.PostAsync(action, null);
                result = await resultPost.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrWhiteSpace(result) || result == "[]")
            {
                return default(T);
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (JsonException exp)
            {
                _logger.Error(result);
                _logger.Error(exp);

                return default(T);
            }
        }

        private string BuildGetUrl(string method, IDictionary<string, object> queryParams)
        {
            string baseUrl;
            if (!string.IsNullOrWhiteSpace(_url))
            {
                baseUrl = _url;
                if (!string.IsNullOrWhiteSpace(method))
                {
                    if (!baseUrl.EndsWith("/"))
                    {
                        baseUrl = $"{baseUrl}/";
                    }

                    baseUrl = $"{baseUrl}{method}";
                }
            }
            else
            {
                baseUrl = method;
            }

            if (queryParams == null || !queryParams.Any())
            {
                return baseUrl;
            }

            var builder = new UriBuilder(baseUrl);
            builder.Port = -1;
            string url = builder.ToString();
            return url;
        }

        private async Task<T> ProcessResult<T>(HttpResponseMessage response)
            where T : class
        {
            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result) || result == "[]")
            {
                return default(T);
            }

            try
            {
                if (typeof(T) == typeof(string))
                {
                    return result as T;
                }

                return JsonConvert.DeserializeObject<T>(result);
            }
            catch (JsonSerializationException)
            {
                return null;
            }
        }

        public static Dictionary<string, string> GetFormContentDictionary(object body)
        {
            var result = new Dictionary<string, string>();
            AddObjToDict(result, body);
            return result;
        }

        private static void FillHeaders(IDictionary<string, object> headers, HttpRequestMessage msg)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    msg.Headers.Add(header.Key, header.Value.ToString());
                }
            }
        }

        private static void AddListToDict(Dictionary<string, string> dict, Proxy arrayItem, string key = null)
        {
            int i = 0;
            var array = arrayItem.Value as System.Collections.IEnumerable;

            foreach (var item in array)
            {
                if (item is System.Collections.IEnumerable && !(item is string))
                {
                    throw new ArgumentException("No embedded arrays in array");
                }

                var name = key ?? $"{arrayItem.Name}[{{0}}]";

                if (item.GetType().IsClass)
                {
                    AddObjToDict(dict, item, string.Format(name, i++) + "[{0}]");
                }
                else
                {
                    dict.Add(string.Format(name, i++), item.ToString());
                }
            }
        }

        private static void AddObjToDict(Dictionary<string, string> dict, object obj, string key = null)
        {
            var type = obj.GetType();

            var nameValueList = type.GetProperties()
                .Select(x => new Proxy { Name = x.Name.ToLowerInvariant(), Value = x.GetValue(obj) })
                .Where(x => x.Value != null);

            foreach (var item in nameValueList)
            {
                var name = key ?? $"{item.Name}";

                if (item.Value is System.Collections.IEnumerable && !(item.Value is string))
                {
                    AddListToDict(dict, item, string.Format(name, item.Name) + "[{0}]");
                }
                else if (item.Value.GetType().IsClass && !(item.Value is string))
                {
                    AddObjToDict(dict, item.Value, string.Format(name, item.Name) + "[{0}]");
                }
                else
                {
                    dict.Add(string.Format(name, item.Name), item.Value.ToString());
                }
            }
        }

        private class Proxy
        {
            public string Name { get; set; }

            public object Value { get; set; }
        }
    }
}