namespace bgTeam.ProjectTemplate
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class Runner
    {
        private readonly IAppSettings _appSettings;

        public Runner(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public Task Run(Dictionary<string, string> cmdParams)
        {
            var s1 = new SolutionGenerator();
            var parsedParams = new CmdParams(cmdParams);

            var settings = new SolutionSettings
            {
                IsWeb = parsedParams.IsWeb ?? true,
                IsApp = parsedParams.IsApp ?? true,
                BgTeamVersion = parsedParams.BgTeamVersion ?? _appSettings.BgTeamVersion,
            };

            s1.Generate(parsedParams.CompanyName ?? _appSettings.NameCompany, parsedParams.ProjectName ?? _appSettings.NameProject, settings);

            return Task.CompletedTask;
        }
    }

    internal class CmdParams
    {
        private const string BgTeamVersionParameter = "bg-team-version";
        private const string CompanyNameParameter = "company";
        private const string ProjectNameParameter = "project";
        private const string IsWebParameter = "is-web";
        private const string IsAppParameter = "is-app";

        public string BgTeamVersion { get; set; }

        public string CompanyName { get; set; }

        public string ProjectName { get; set; }

        public bool? IsWeb { get; set; }

        public bool? IsApp { get; set; }

        public CmdParams(Dictionary<string, string> cmdParams)
        {
            if (cmdParams.TryGetValue(BgTeamVersionParameter, out string bgTeamVersion))
            {
                BgTeamVersion = bgTeamVersion;
            }

            if (cmdParams.TryGetValue(CompanyNameParameter, out string companyName))
            {
                CompanyName = companyName;
            }

            if (cmdParams.TryGetValue(ProjectNameParameter, out string projectName))
            {
                ProjectName = projectName;
            }

            if (cmdParams.TryGetValue(IsWebParameter, out string isWeb))
            {
                IsWeb = bool.Parse(isWeb);
            }

            if (cmdParams.TryGetValue(IsAppParameter, out string isApp))
            {
                IsApp = bool.Parse(isApp);
            }
        }
    }
}
