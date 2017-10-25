﻿namespace bgTeam.Core.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;

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

        public string this[string key] => _configurationRoot[key];

        public string GetConnectionString(string name)
        {
            return _configurationRoot.GetConnectionString(name);
        }

        public IConfigurationSection GetSection(string name)
        {
            return _configurationRoot.GetSection(name);
        }

        private IConfigurationRoot Initialize(string appsettings, string envVariable = null)
        {
            var curDir = Environment.CurrentDirectory;
            var appsettingsFile = File.ReadAllText(Path.Combine(curDir, $"{appsettings}.json"));
            var mainConf = JsonConvert.DeserializeObject<SettingsProxy>(appsettingsFile);
            var buildConfigure = envVariable ?? Environment.GetEnvironmentVariable(mainConf.BuildConfigureVariable);
            var appsettingsFileAdd = Path.Combine(curDir, $"{appsettings}.{buildConfigure}.json");

            var builder = new ConfigurationBuilder()
                .SetBasePath(curDir)
                .AddJsonFile($"{appsettings}.json");

            if (buildConfigure == null)
            {
                throw new Exception($"You must set Environment variable: {mainConf.BuildConfigureVariable}");
            }

            if (File.Exists(appsettingsFileAdd))
            {
                builder.AddJsonFile(appsettingsFileAdd);
            }

            var secondConf = builder.Build();

            var confsPath = secondConf["ConfigsPath"];

            if (!string.IsNullOrEmpty(mainConf.AdditionalConfigs))
            {
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

            public string ConfigsPath { get; set; } = string.Empty;

            public string AdditionalConfigs { get; set; } = string.Empty;
        }
    }
}
