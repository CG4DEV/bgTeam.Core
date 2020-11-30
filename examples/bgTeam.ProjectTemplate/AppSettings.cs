namespace bgTeam.ProjectTemplate
{
    using Microsoft.Extensions.Configuration;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649", Justification = "Reviewed")]
    internal interface IAppSettings
    {
        string NameCompany { get; set; }

        string NameProject { get; set; }

        string BgTeamVersion { get; set; }
    }

    internal class AppSettings : IAppSettings
    {
        public AppSettings(IConfiguration config)
        {
            NameCompany = config[nameof(NameCompany)];

            NameProject = config[nameof(NameProject)];

            BgTeamVersion = config[nameof(BgTeamVersion)];
        }

        public string NameCompany { get; set; }

        public string NameProject { get; set; }

        public string BgTeamVersion { get; set; }
    }
}
