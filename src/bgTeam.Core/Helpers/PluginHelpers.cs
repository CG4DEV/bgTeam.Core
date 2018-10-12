namespace bgTeam.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Helpers function for plugins
    /// </summary>
    public static class PluginHelpers
    {
        /// <summary>
        /// Загружает информацию о типах из внешних библиотек dll. Производит позднее связывание
        /// </summary>
        /// <typeparam name="T">Искомый тип</typeparam>
        /// <param name="path">Путь к директории с библиотеками</param>
        /// <returns>Возвращает коллекцию найденных типов</returns>
        public static IEnumerable<Type> Load<T>(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                return Enumerable.Empty<Type>();
            }

            var plugins = dirInfo.GetFiles("*.dll")
                .Select(x => Assembly.Load(x.FullName))
                .Where(a => a.GetTypes().Any(x => TypesPredicate<T>(x)));

            if (!plugins.Any())
            {
                return Enumerable.Empty<Type>();
            }

            var types = new List<Type>();
            foreach (var plugin in plugins)
            {
                var loadedTypes = plugin.GetTypes()
                    .Where(x => TypesPredicate<T>(x))
                    .ToArray();
                types.AddRange(loadedTypes);
            }

            return types;
        }

        private static bool TypesPredicate<T>(Type inputType)
        {
            var targetType = typeof(T);

            if (targetType.IsInterface)
            {
                return targetType.IsAssignableFrom(inputType);
            }

            if (targetType.IsAbstract)
            {
                return inputType.IsClass && !inputType.IsAbstract && inputType.IsSubclassOf(targetType);
            }

            if (targetType.IsClass)
            {
                return inputType.IsClass && (inputType.Equals(targetType) || targetType.IsSubclassOf(targetType));
            }

            return false;
        }
    }
}
