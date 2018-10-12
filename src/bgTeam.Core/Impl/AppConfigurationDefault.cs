namespace bgTeam.Core.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using bgTeam.Core;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Реализация по-умолчанию реализации класса для получения значений из конфигурационного файла.
    /// </summary>
    public class AppConfigurationDefault : IAppConfiguration
    {
        private readonly IConfigurationRoot _configurationRoot;

        public AppConfigurationDefault()
        {
            _configurationRoot = Initialize("appsettings");
        }

        public AppConfigurationDefault(string name)
        {
            _configurationRoot = Initialize(name, null);
        }

        public AppConfigurationDefault(string name, string envVariable)
        {
            _configurationRoot = Initialize(name, envVariable);
        }

        /// <summary>
        /// Получение значения настройки по её названию key.
        /// </summary>
        /// <param name="key">Название настройки.</param>
        /// <returns>Строка. Значение настройки.</returns>
        public string this[string key] => _configurationRoot[key];

        /// <summary>
        /// Возвращает строку подключения по её названию в конфигаруционном файле.
        /// </summary>
        /// <param name="name">Название строки подключения.</param>
        /// <returns>Строку подключения.</returns>
        public string GetConnectionString(string name)
        {
            return _configurationRoot.GetConnectionString(name);
        }

        /// <summary>
        /// Возвращает секцию настроек по её имени.
        /// </summary>
        /// <param name="name">Название секции настроек.</param>
        /// <returns></returns>
        public IConfigurationSection GetSection(string name)
        {
            return _configurationRoot.GetSection(name);
        }

        private IConfigurationRoot Initialize(string fileConfiguration, string envVariable = null)
        {
            string appsettingsFile;
            var curDir = Environment.CurrentDirectory;

            try
            {
                appsettingsFile = File.ReadAllText(Path.Combine(curDir, $"{fileConfiguration}.json"));
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException($"Not find config file - {fileConfiguration}.json, path - {curDir}\\");
            }

            var builder = new ConfigurationBuilder()
                    .SetBasePath(curDir)
                    .AddJsonFile($"{fileConfiguration}.json");

            var mainConf = JsonConvert.DeserializeObject<SettingsProxy>(appsettingsFile);
            var buildConfigure = envVariable ?? mainConf.AppEnvironment;

            // если нашли доп. конфигурации
            if (!string.IsNullOrEmpty(buildConfigure))
            {
                SettingsProxy addtionalConf = null;
                var appsettingsFileAdd = Path.Combine(curDir, $"{fileConfiguration}.{buildConfigure}.json");
                if (File.Exists(appsettingsFileAdd))
                {
                    builder.AddJsonFile(appsettingsFileAdd);
                    addtionalConf = JsonConvert.DeserializeObject<SettingsProxy>(File.ReadAllText(appsettingsFileAdd));
                }

                //var secondConf = builder.Build();
                var confsPath = string.IsNullOrEmpty(addtionalConf?.AppConfigsPath) ? mainConf.AppConfigsPath : addtionalConf.AppConfigsPath;
                var appConfigsAdditional = string.IsNullOrEmpty(addtionalConf?.AppConfigsAdditional) ? mainConf.AppConfigsAdditional : addtionalConf.AppConfigsAdditional;

                if (!string.IsNullOrEmpty(appConfigsAdditional))
                {
                    if (string.IsNullOrEmpty(confsPath))
                    {
                        throw new ArgumentNullException(nameof(mainConf.AppConfigsPath));
                    }

                    foreach (var item in appConfigsAdditional.Split(',').Select(x => x.Trim()))
                    {
                        builder.AddJsonFile(Path.Combine(confsPath, $"{item}.{buildConfigure}.json"));
                    }
                }
            }

            builder.AddEnvironmentVariables();

            return builder.Build();
        }

        private class SettingsProxy
        {
            public string AppEnvironment { get; set; } = string.Empty;

            public string AppConfigsPath { get; set; } = string.Empty;

            public string AppConfigsAdditional { get; set; } = string.Empty;
        }
    }
}
