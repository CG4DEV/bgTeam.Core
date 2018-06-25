namespace bgTeam.Core.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

    /// <summary>
    /// Реализация по-умолчанию реализации класса для получения значений из конфигурационного файла
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
        /// Получение значения настройки по её названию key
        /// </summary>
        /// <param name="key">Название настройки</param>
        /// <returns>Строка. Значение настройки</returns>
        public string this[string key] => _configurationRoot[key];

        /// <summary>
        /// Возвращает строку подключения по её названию в конфигаруционном файле
        /// </summary>
        /// <param name="name">Название строки подключения</param>
        /// <returns>Строку подключения</returns>
        public string GetConnectionString(string name)
        {
            return _configurationRoot.GetConnectionString(name);
        }

        /// <summary>
        /// Возвращает секцию настроек по её имени
        /// </summary>
        /// <param name="name">Название секции настроек</param>
        /// <returns></returns>
        public IConfigurationSection GetSection(string name)
        {
            return _configurationRoot.GetSection(name);
        }

        private IConfigurationRoot Initialize(string appsettings, string envVariable = null)
        {
            var curDir = Environment.CurrentDirectory;
            string appsettingsFile;

            try
            {
                appsettingsFile = File.ReadAllText(Path.Combine(curDir, $"{appsettings}.json"));
            }
            catch (FileNotFoundException)
            {
                throw new ArgumentException($"Not find config file - {appsettings}.json, path - {curDir}\\");
            }

            var mainConf = JsonConvert.DeserializeObject<SettingsProxy>(appsettingsFile);
            var buildConfigure = envVariable ?? Environment.GetEnvironmentVariable(mainConf.BuildConfigureVariable) ?? string.Empty;

            var builder = new ConfigurationBuilder()
                    .SetBasePath(curDir)
                    .AddJsonFile($"{appsettings}.json");

            //if (buildConfigure == null)
            //{
            //    throw new Exception($"You must set Environment variable: {mainConf.BuildConfigureVariable}");
            //}

            // если нашли доп. конфигурации
            if (!string.IsNullOrEmpty(buildConfigure))
            {
                var appsettingsFileAdd = Path.Combine(curDir, $"{appsettings}.{buildConfigure}.json");
                if (File.Exists(appsettingsFileAdd))
                {
                    builder.AddJsonFile(appsettingsFileAdd);
                }
            }

            var secondConf = builder.Build();

            var confsPath = secondConf["ConfigsPath"];

            if (!string.IsNullOrEmpty(mainConf.AdditionalConfigs))
            {
                if (string.IsNullOrEmpty(confsPath))
                {
                    throw new ArgumentNullException("ConfigsPath");
                }

                foreach (var item in mainConf.AdditionalConfigs.Split(',').Select(x => x.Trim()))
                {
                    builder.AddJsonFile(Path.Combine(confsPath, $"{item}.{buildConfigure}.json"));
                }
            }

            return builder.Build();
        }

        private class SettingsProxy
        {
            public string BuildConfigureVariable { get; set; } = string.Empty;

            public string AdditionalConfigs { get; set; } = string.Empty;
        }
    }
}
