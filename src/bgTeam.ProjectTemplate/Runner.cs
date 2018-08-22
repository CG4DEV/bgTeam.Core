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

            s1.Generate(_appSettings.NameCompany, _appSettings.NameProject,
                new SolutionSettings
                {
                    IsWeb = true,
                    IsApp = true,
                    BgTeamVersion = _appSettings.BgTeamVersion
                });
        }
    }
}
