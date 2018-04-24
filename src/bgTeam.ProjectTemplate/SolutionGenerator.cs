using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace bgTeam.ProjectTemplate
{
    public class SolutionGenerator
    {
        private string bgTeamVersion = "2.0.6-beta";
        private string _namesmall;

        public void Generate(string name)
        {
            _namesmall = name;

            var folder = "result";

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            var projects = GenerateProjects($"bgTeam.{name}", folder, "src");

            var file = ProjectsFile(projects);

            File.WriteAllText($"{folder}\\bgTeam.{name}.sln", file);

            Directory.Move("./Resourse/lint", $"{folder}/lint");
        }

        private IEnumerable<ProjectInfoItem> GenerateProjects(string name, string folder, string path)
        {
            var result = new List<ProjectInfoItem>();
            var fullPath = $"{folder}\\{path}";

            var p1 = new ProjectGenerator($"{name}.Common", fullPath);
            p1.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Core", bgTeamVersion) });
            p1.Folder("Impl");
            p1.ClassTemplateFile("ITestService", "Common\\ITestService");
            p1.ClassTemplateFile("TestService", "Common\\TestService", new[] { "Impl" });
            result.Add(new ProjectInfoItem(p1.Name, $"{path}\\{p1.Path}"));

            var p2 = new ProjectGenerator($"{name}.DataAccess", fullPath);
            p2.ProjectFile(new NugetItem[] 
                {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.DataAccess", bgTeamVersion)
                }, new string[] { $"{name}.Domain" });
            p2.Folder("Impl");
            p2.ClassTemplateFile("TestQuery", "DataAccess\\TestQuery", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", _namesmall) });
            p2.ClassTemplateFile("TestQueryContext", "DataAccess\\TestQueryContext");
            result.Add(new ProjectInfoItem(p2.Name, $"{path}\\{p2.Path}"));

            var p3 = new ProjectGenerator($"{name}.Domain", fullPath);
            p3.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Impl.Dapper", bgTeamVersion) });
            p3.Folder("Dto");
            p3.Folder("Entities");
            p3.ClassTemplateFile("IEntity", "Domain\\IEntity");
            p3.ClassTemplateFile("Test", "Domain\\Test", new[] { "Entities" });
            p3.ClassTemplateFile("TestDto", "Domain\\TestDto", new[] { "Dto" });
            result.Add(new ProjectInfoItem(p3.Name, $"{path}\\{p3.Path}"));

            var p4 = new ProjectGenerator($"{name}.Story", fullPath);
            p4.ProjectFile(new NugetItem[]
                {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.DataAccess", bgTeamVersion)
                }, new string[] { $"{name}.Domain" });
            p4.ClassTemplateFile("TestStory", "Story\\TestStory", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", _namesmall) });
            p4.ClassTemplateFile("TestStoryContext", "Story\\TestStoryContext");
            p4.ClassTemplateFile("IStoryLibrary", "Story\\IStoryLibrary");
            p4.ClassTemplateFile("AutoMapperStory", "Story\\AutoMapperStory");
            result.Add(new ProjectInfoItem(p4.Name, $"{path}\\{p4.Path}"));

            var p5 = new ProjectGenerator($"{name}.App", fullPath);
            p5.ProjectFile(new NugetItem[]
                {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.Impl.Dapper", bgTeamVersion),
                    new NugetItem("bgTeam.Impl.MsSql", bgTeamVersion),
                    new NugetItem("Scrutor", "2.1.2")
                }, new string[] { $"{name}.Story" });
            p5.ClassTemplateFile("AppSettings", "App\\AppSettings");
            p5.ClassTemplateFile("Program", "App\\Program", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", _namesmall) });
            p5.ClassTemplateFile("Runner", "App\\Runner", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", _namesmall) });
            p5.JsonTemplateFile("appsettings", "App\\appsettings");
            result.Add(new ProjectInfoItem(p5.Name, $"{path}\\{p5.Path}"));

            return result;
        }

        private string ProjectsFile(IEnumerable<ProjectInfoItem> projects)
        {
            var str = new StringBuilder();

            str.AppendLine(
@"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 15
VisualStudioVersion = 15.0.27130.2036
MinimumVisualStudioVersion = 10.0.40219.1");

            var slGuid = "{9A19103F-16F7-4668-BE54-9A1E7A4F7556}";
            foreach (var prj in projects)
            {
                str.AppendLine($"Project(\"{slGuid}\") = \"{prj.Name}\", \"{prj.Path}\", \"{{{prj.Code}}}\"");
                str.AppendLine("EndProject");
            }

            str.AppendLine("Global");
            str.AppendLine(
@"	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Live|Any CPU = Live|Any CPU
		Release|Any CPU = Release|Any CPU
		Uat|Any CPU = Uat|Any CPU
	EndGlobalSection");

            str.AppendLine("	GlobalSection(ProjectConfigurationPlatforms) = postSolution");
            foreach (var prj in projects)
            {
                str.AppendLine($"    	{{{prj.Code}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Debug|Any CPU.Build.0 = Debug|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Live|Any CPU.ActiveCfg = Live|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Live|Any CPU.Build.0 = Live|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Release|Any CPU.ActiveCfg = Release|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Release|Any CPU.Build.0 = Release|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Uat|Any CPU.ActiveCfg = Uat|Any CPU");
                str.AppendLine($"		{{{prj.Code}}}.Uat|Any CPU.Build.0 = Uat|Any CPU");
            }
            str.AppendLine("	EndGlobalSection");

            str.AppendLine(
@"	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
	GlobalSection(ExtensibilityGlobals) = postSolution
		SolutionGuid = {1CEBAF23-BD02-4EA1-92B0-AF727714CF50}
	EndGlobalSection");

            str.AppendLine("EndGlobal");

            return str.ToString();
        }
    }










    class ProjectInfoItem
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public Guid Code { get; set; }

        public ProjectInfoItem(string name, string path)
        {
            Name = name;
            Path = path;

            Code = Guid.NewGuid();
        }
    }
}
