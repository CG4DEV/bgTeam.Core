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
}
