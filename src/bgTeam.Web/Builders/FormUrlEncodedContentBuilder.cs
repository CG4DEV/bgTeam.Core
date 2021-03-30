﻿namespace bgTeam.Web.Builders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;

    /// <summary>
    /// Generates HttpContent using any object as source
    /// </summary>
    public class FormUrlEncodedContentBuilder : IContentBuilder
    {
        public static Dictionary<string, string> GetFormContentDictionary(object body)
        {
            var result = new Dictionary<string, string>();
            AddObjToDict(result, body);
            return result;
        }

        /// <summary>
        /// Builds HttpContent object from any object
        /// </summary>
        /// <param name="param">Anonymus or any another object</param>
        /// <returns></returns>
        public HttpContent Build(object param)
        {
            var dic = GetFormContentDictionary(param);
            return new FormUrlEncodedContent(dic);
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

                if (item.GetType().IsClass && !(item is string))
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
                .Where(x => x.Value != null)
                .ToArray();

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
