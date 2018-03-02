namespace bgTeam.Web.Impl
{
    using bgTeam.Extensions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class WebClient : IWebClient
    {
        private readonly string _url;

        public WebClient(string url)
        {
            _url = url.CheckNull(nameof(url));
        }

        public string Url => _url;

        public async Task<T> GetAsync<T>(string method, IDictionary<string, object> queryParams = null, IDictionary<string, object> headers = null)
            where T : class
        {
            string url = BuildGetUrl(method, queryParams);
            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            FillHeaders(headers, msg);

            using (var client = new HttpClient())
            {
                var resultGet = await client.SendAsync(msg);

                return await ProcessResult<T>(resultGet);
            }
        }

        public async Task<T> PostAsync<T>(string action, object postParams)
            where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_url);
                var dic = GetFormContentDictionary(postParams);
                var content = new FormUrlEncodedContent(dic);

                var resultPost = await client.PostAsync(action, content);
                var result = await resultPost.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(result) || result == "[]")
                {
                    return default(T);
                }

                try
                {
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (JsonSerializationException)
                {
                    return null;
                }
            }
        }

        private string BuildGetUrl(string method, IDictionary<string, object> queryParams)
        {
            var baseUrl = _url;
            if (!string.IsNullOrWhiteSpace(method))
            {
                if (!baseUrl.EndsWith("/"))
                {
                    baseUrl = $"{baseUrl}/";
                }

                baseUrl = $"{baseUrl}{method}";
            }

            if (queryParams == null || !queryParams.Any())
            {
                return baseUrl;
            }

            var builder = new UriBuilder(baseUrl);
            builder.Port = -1;
            //var query = ParseQueryString(builder.Query);

            //foreach (var param in query)
            //{
            //    query[param.Key] = param.Value.ToString();
            //}

            //builder.Query = queryParams.ToString();
            string url = builder.ToString();
            return url;
        }

        //TODO : пришлось удалить HttpUtility.ParseQueryString(builder.Query);
        //не собирался проект
        private IDictionary<string, object> ParseQueryString(string query)
        {
            if (query.NullOrEmpty())
            {
                return new Dictionary<string, object>();
            }

            throw new NotImplementedException($"ParseQueryString - {query}");
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
                    //if (header.Key.ToLower().Equals("content-type"))
                    //{
                    //    msg.Content.Headers.ContentType = new MediaTypeHeaderValue(header.Value.ToString());
                    //}
                    //else
                    //{
                    //    
                    //}

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
