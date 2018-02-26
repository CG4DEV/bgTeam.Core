namespace bgTeam.Core
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Интерфейс для получения значений из конфигурационного файла
    /// </summary>
    public interface IAppConfiguration
    {
        string this[string key] { get; }

        string GetConnectionString(string name);

        IConfigurationSection GetSection(string name);
    }
}
