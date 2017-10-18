namespace bgTeam.Core
{
    using Microsoft.Extensions.Configuration;

    public interface IAppConfiguration
    {
        string this[string key] { get; }

        string GetConnectionString(string name);

        IConfigurationSection GetSection(string name);
    }
}
