namespace bgTeam.Infrastructure
{
    using bgTeam.Core;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;

    public class SerilogConfig : IAppLoggerConfig
    {
        private readonly IAppConfiguration _appConfiguration;

        private readonly Dictionary<string, string> _defaultConfig = new Dictionary<string, string>()
        {
            ["Serilog:MinimumLevel"] = "Verbose",
            ["Serilog:Using:0"] = "Serilog.Sinks.ColoredConsole",

            ["Serilog:WriteTo:0:Name"] = "ColoredConsole",
            ["Serilog:WriteTo:0:Args:outputTemplate"] = "{Timestamp:yyyy.MM.dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}",

            ["Serilog:WriteTo:1:Name"] = "RollingFile",
            ["Serilog:WriteTo:1:Args:pathFormat"] = "log-{Date}.log",
            ["Serilog:WriteTo:1:Args:outputTemplate"] = "{Timestamp:yyyy.MM.dd HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}"
        };

        public SerilogConfig()
        {

        }

        public SerilogConfig(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
        }

        public string LoggerName => "App.Main.Logger";

        public IConfigurationSection GetLoggerConfig()
        {
            var conf = _appConfiguration?.GetSection("Serilog");
            if (conf != null)
            {
                return conf;
            }

            return new ConfigurationBuilder()
                .AddInMemoryCollection(_defaultConfig)
                .Build()
                .GetSection("Serilog");
        }
    }
}
