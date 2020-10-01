namespace bgTeam.ProjectTemplate
{

    public class SolutionSettings
    {
        public bool IsWeb { get; internal set; }

        public bool IsApp { get; internal set; }

        public string BgTeamVersion { get; set; }

        public string EntityFrameworkCoreVersion { get; set; } = "3.1.8";

        public string EntityFrameworkCoreNpgsqlVersion { get; set; } = "3.1.4";
    }
}
