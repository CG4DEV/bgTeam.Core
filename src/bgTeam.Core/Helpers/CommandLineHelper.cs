namespace bgTeam.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using bgTeam.Extensions;

    public static class CommandLineHelper
    {
        /// <summary>
        /// Разделает параметры строки -key1 value1 -key2 value2. Отсутствие value == null
        /// </summary>
        /// <param name="args">Параметры коммандной строки</param>
        /// <param name="keyPrefix">Префикс ключа</param>
        /// <returns><see cref="Dictionary{string, string}"/></returns>
        public static Dictionary<string, string> ParseArgs(string[] args, string keyPrefix = "-")
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Dictionary<string, string> dict = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            int i = 0;

            while (i < args.Length)
            {
                var key = args[i];

                if (!key.StartsWith(keyPrefix))
                {
                    throw new ArgumentException($"Строка параметров имеет не верный формат: {key}, ожидалось: {keyPrefix}{key}", nameof(keyPrefix));
                }
                else
                {
                    key = key.Substring(keyPrefix.Length);
                }

                i++;
                var value = i < args.Length ? args[i] : null;

                if (value != null && !value.StartsWith(keyPrefix))
                {
                    i++;
                    dict.Add(key, value);
                }
                else
                {
                    dict.Add(key, null);
                }
            }

            return dict;
        }

        /// <summary>
        /// Формирует инстанс класса аргументов по переданным ключам
        /// </summary>
        /// <typeparam name="T">Класс аргументов</typeparam>
        /// <param name="mappingDictionary">Словарь маппинг ключ-поле</param>
        /// <param name="args">Переданные аргументы</param>
        /// <returns></returns>
        public static T CreateArgsInstance<T>(Dictionary<string, string> mappingDictionary, string[] args)
            where T : class, new()
        {
            var lowArgs = args.Select(x => x.ToLowerInvariant()).ToArray();
            var argsDic = mappingDictionary.Where(x => Array.IndexOf(lowArgs, x.Key) != -1)
                 .Select(x => new
                 {
                     Key = x.Value,
                     Value = args[Array.IndexOf(lowArgs, x.Key) + 1],
                 })
                 .ToDictionary(x => x.Key, x => x.Value);

            var result = Activator.CreateInstance<T>();
            foreach (var item in argsDic)
            {
                result.SetProperty(item.Key, item.Value);
            }

            return result;
        }
    }
}
