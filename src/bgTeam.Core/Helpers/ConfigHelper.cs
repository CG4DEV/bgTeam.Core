namespace bgTeam.Core.Helpers
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Helpers function for configs files
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// Считать конфигурацию
        /// </summary>
        public static IEnumerable<T> Init<T>(string path)
            where T : new()
        {
            var files = Directory.GetFiles(path, "*.json");

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
