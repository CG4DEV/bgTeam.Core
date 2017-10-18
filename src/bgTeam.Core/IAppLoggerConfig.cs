namespace bgTeam.Core
{
    using Microsoft.Extensions.Configuration;

    public interface IAppLoggerConfig
    {
        string LoggerName { get; }

        IConfigurationSection GetLoggerConfig();
    }
}
