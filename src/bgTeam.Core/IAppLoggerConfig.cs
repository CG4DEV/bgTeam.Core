namespace bgTeam.Core
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Интерфейс для получения конфигурации при формировании логгера
    /// </summary>
    public interface IAppLoggerConfig
    {
        string LoggerName { get; }

        IConfigurationSection GetLoggerConfig();
    }
}
