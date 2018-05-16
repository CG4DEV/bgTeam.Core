using System;
using Xunit;

namespace bgTeam.ProjectTemplate.Tests
{
    public class UnitTest1
    {
        private string bgTeamVersion = "2.0.5-alfa";

        public UnitTest1()
        {

        }

        [Fact]
        public void Generator_ProjectFile()
        {
            var name = "bgTeam.TestGenerate";
            var path = "result";

            var p1 = new ProjectGenerator($"{name}.Common", path);

            p1.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Core", bgTeamVersion) });
            p1.Folder("Impl");
            //g.ClassTemplateFile();

            var p2 = new ProjectGenerator($"{name}.DataAccess", path);
            p2.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.DataAccess", bgTeamVersion) });
            p2.Folder("Impl");

            var p3 = new ProjectGenerator($"{name}.Domain", path);
            p3.ProjectFile(new NugetItem[] { });

            var p4 = new ProjectGenerator($"{name}.Story", path);
            p4.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Core", bgTeamVersion) });

            var p5 = new ProjectGenerator($"{name}.App", path);
            p4.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Core", bgTeamVersion) });
        }

        [Fact]
        public void Generator_SolutionGenerator()
        {
            var s1 = new SolutionGenerator();

            s1.Generate("Sso");
        }
    }
}
