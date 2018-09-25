namespace bgTeam.ProjectTemplate
{
    using System.Threading.Tasks;

    internal class Runner
    {
        private readonly IAppSettings _appSettings;

        public Runner(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task Run()
        {
            var s1 = new SolutionGenerator();

            var settings = new SolutionSettings
            {
                IsWeb = true,
                IsApp = true,
                BgTeamVersion = _appSettings.BgTeamVersion
            };

            s1.Generate(_appSettings.NameCompany, _appSettings.NameProject, settings);
        }
    }
}
