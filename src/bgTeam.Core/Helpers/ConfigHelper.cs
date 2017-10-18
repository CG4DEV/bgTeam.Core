namespace bgTeam.Core.Helpers
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class ConfigHelper
    {
        /// <summary>
        /// Считать конфигурацию
        /// </summary>
        public static IEnumerable<T> Init<T>(string path)
            where T : new()
        {
            var files = Directory.GetFiles(path);

            if (!files.Any())
            {
                return Enumerable.Empty<T>();
            }

            var list = new List<T>();
            foreach (var item in files)
            {
                var str = File.ReadAllText(item);
                list.AddRange(JsonConvert.DeserializeObject<T[]>(str));
            }

            return list;
        }
    }
}
