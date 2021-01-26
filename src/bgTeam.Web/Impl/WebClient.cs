namespace bgTeam.Web.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using System.Web;
    using bgTeam.Extensions;
    using bgTeam.Web.Builders;
    using bgTeam.Web.Exceptions;
    using Newtonsoft.Json;

    public class WebClient : IWebClient
    {
        private readonly string _url;
        private readonly HttpClient _client;
        private readonly IContentBuilder _builder;

        private readonly SocketsHttpHandler _handler;

        public WebClient(string url)
            : this(url, new FormUrlEncodedContentBuilder())
        {
        }

        public WebClient(string url, X509CertificateCollection clientCertificates)
            : this(url, new FormUrlEncodedContentBuilder())
        {
            var sslOptions = new System.Net.Security.SslClientAuthenticationOptions();
            sslOptions.ClientCertificates = clientCertificates;
            sslOptions.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;

            _handler.SslOptions = sslOptions;
        }

        public WebClient(string url, IContentBuilder builder)
        {
            _url = url;
            _builder = builder.CheckNull(nameof(builder));

            _handler = new SocketsHttpHandler();
            _client = new HttpClient(_handler);

            ConnectionsLimit = 1024;
            MaxIdleTime = 300000; // 5 мин
            ConnectionLeaseTimeout = 0; // закрываем соединение сразу после выполнения запроса

            Culture = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Количество одновременных запросов на удалённый сервер. По-умолчанию для .net core int.Max, в остальных случаях 2
        /// </summary>
        public int ConnectionsLimit
        {
            get
            {
                return _handler.MaxConnectionsPerServer;
            }

            set
            {
                _handler.MaxConnectionsPerServer = value;
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
                return Convert.ToInt32(_handler.PooledConnectionIdleTimeout.TotalMilliseconds);
            }

            set
            {
                _handler.PooledConnectionIdleTimeout = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// Указывает, сколько времени (в мс) соединение может удерживаться открытым. По умолчанию лимита времени жизни для соединений нет. Установка его в 0 приведет к тому, что каждое соединение будет закрываться сразу после выполнения запроса.
        /// </summary>
        public int ConnectionLeaseTimeout
        {
            get
            {
                return Convert.ToInt32(_handler.PooledConnectionLifetime.TotalMilliseconds);
            }

            set
            {
                _handler.PooledConnectionLifetime = TimeSpan.FromMilliseconds(value);
            }
        }

        /// <summary>
        /// Culture for query builder. <see cref="CultureInfo.CurrentCulture"/>.
        /// </summary>
        public CultureInfo Culture { get; set; }

        /// <summary>
        /// Устанавливает или возвращает таймаут запроса к серверу
        /// </summary>
        public TimeSpan RequestTimeout
        {
            get { return _client.Timeout; }
            set { _client.Timeout = value; }
        }

        public string Url => _url;

        public void SetAuthHeader(string scheme, string value)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, value);
        }

        public virtual async Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            var result = await GetResponseAsync(method, queryParams, headers);
            return await ProcessResultAsync<T>(result);
        }

        public virtual async Task GetAsync(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
        {
            var result = await GetResponseAsync(method, queryParams, headers);
            CheckResult(result);
        }

        public virtual async Task<T> DeleteAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            var result = await GetDeleteResponseAsync(method, queryParams, headers);
            return await ProcessResultAsync<T>(result);
        }

        public virtual async Task DeleteAsync(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
        {
            var result = await GetDeleteResponseAsync(method, queryParams, headers);
            CheckResult(result);
        }

        public virtual async Task<T> PostAsync<T>(string method, object postParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            var result = await GetPostResponseAsync(method, postParams, headers);
            return await ProcessResultAsync<T>(result);
        }

        public virtual async Task PostAsync(string method, object postParams = null, IDictionary<string, object> headers = null)
        {
            var result = await GetPostResponseAsync(method, postParams, headers);
            CheckResult(result);
        }

        public virtual async Task<T> PutAsync<T>(string method, object putParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            var result = await GetPutResponseAsync(method, putParams, headers);
            return await ProcessResultAsync<T>(result);
        }

        public virtual async Task PutAsync(string method, object putParams = null, IDictionary<string, object> headers = null)
        {
            var result = await GetPutResponseAsync(method, putParams, headers);
            CheckResult(result);
        }

        private Task<HttpResponseMessage> GetResponseAsync(string method, IDictionary<string, object> queryParams, IDictionary<string, object> headers)
        {
            HttpRequestMessage msg = BuildHttpRequest(method, queryParams, headers, HttpMethod.Get);
            return _client.SendAsync(msg);
        }

        private Task<HttpResponseMessage> GetDeleteResponseAsync(string method, IDictionary<string, object> queryParams, IDictionary<string, object> headers)
        {
            HttpRequestMessage msg = BuildHttpRequest(method, queryParams, headers, HttpMethod.Delete);
            return _client.DeleteAsync(msg.RequestUri);
        }

        private Task<HttpResponseMessage> GetPostResponseAsync(string method, object postParams, IDictionary<string, object> headers)
        {
            HttpContent content = BuildContent(postParams, headers);

            var url = BuildUrl(method);
            return _client.PostAsync(url, content);
        }

        private Task<HttpResponseMessage> GetPutResponseAsync(string method, object putParams, IDictionary<string, object> headers)
        {
            HttpContent content = BuildContent(putParams, headers);

            var url = BuildUrl(method);
            return _client.PutAsync(url, content);
        }

        private HttpRequestMessage BuildHttpRequest(string method, IDictionary<string, object> queryParams, IDictionary<string, object> headers, HttpMethod requestType)
        {
            if (string.IsNullOrWhiteSpace(_url))
            {
                method.CheckNull(nameof(method));
            }

            string url = BuildUrl(method, queryParams);
            var msg = new HttpRequestMessage(requestType, url);
            FillHeaders(headers, msg);
            return msg;
        }

        private HttpContent BuildContent(object requestParams, IDictionary<string, object> headers)
        {
            HttpContent content = null;
            if (requestParams != null)
            {
                content = _builder.Build(requestParams);
            }

            if (!headers.NullOrEmpty())
            {
                if (content == null)
                {
                    content = new StringContent(string.Empty);
                }

                FillContentHeaders(headers, content);
            }

            return content;
        }

        private void CheckResult(HttpResponseMessage resultPost)
        {
            switch (resultPost.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.NoContent:
                case HttpStatusCode.PartialContent:
                    {
                        return;
                    }

                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadGateway:
                case HttpStatusCode.ServiceUnavailable:
                case HttpStatusCode.GatewayTimeout:
                    {
                        throw new WebClientException($"Status code: {resultPost.StatusCode}. Message: {resultPost.ReasonPhrase}", resultPost.StatusCode);
                    }

                default:
                    {
                        throw new WebClientException($"Unknown http error. Status code: {resultPost.StatusCode}. Message {resultPost.ReasonPhrase}", resultPost.StatusCode);
                    }
            }
        }

        private void FillContentHeaders(IDictionary<string, object> headers, HttpContent content)
        {
            foreach (var header in headers)
            {
                content.Headers.Add(header.Key, header.Value.ToString());
            }
        }

        private string BuildUrl(string method, IDictionary<string, object> queryParams = null)
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

                    if (method.StartsWith("/"))
                    {
                        method = method.Substring(1);
                    }

                    baseUrl = $"{baseUrl}{method}";
                }
            }
            else
            {
                baseUrl = method;
            }

            if (queryParams.NullOrEmpty())
            {
                return baseUrl;
            }

            var builder = new UriBuilder(baseUrl);

            var query = HttpUtility.ParseQueryString(builder.Query);
            queryParams.DoForEach(x => query.Add(x.Key, ObjectToQueryStringValue(x.Value)));
            builder.Query = query.ToString();

            string url = builder.ToString();
            return url;
        }

        private string ObjectToQueryStringValue(object o)
        {
            switch (o)
            {
                case string str:
                    return str;
                case DateTime dateTime:
                    return dateTime.ToString(Culture);
                case DateTimeOffset dateTimeOffset:
                    return dateTimeOffset.ToString(Culture);
                case TimeSpan timeSpan:
                    return timeSpan.ToString("G", Culture);
                case double dbl:
                    return dbl.ToString(Culture);
                case decimal dcml:
                    return dcml.ToString(Culture);
                case float flt:
                    return flt.ToString(Culture);
                default:
                    return o.ToString();
            }
        }

        private async Task<T> ProcessResultAsync<T>(HttpResponseMessage response)
            where T : class
        {
            CheckResult(response);

            if (response.Content.Headers.ContentType != null
                && string.Equals(response.Content.Headers.ContentType.CharSet, "utf8", StringComparison.OrdinalIgnoreCase))
            {
                response.Content.Headers.ContentType.CharSet = "utf-8";
            }

            var result = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(result))
            {
                return default(T);
            }

            if (typeof(T) == typeof(string))
            {
                return result as T;
            }

            return JsonConvert.DeserializeObject<T>(result);
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
    }
}