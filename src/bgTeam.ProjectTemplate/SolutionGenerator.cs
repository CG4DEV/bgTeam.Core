using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using bgTeam.Extensions;

namespace bgTeam.ProjectTemplate
{
    public class SolutionGenerator
    {
        private string bgTeamVersion = "2.0.6-beta";
        //private string _namesmall;

        public void Generate(string @namespace, string name, SolutionSettings settings)
        {
            //_namesmall = name;

            var folder = "result";

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            var projects = GenerateProjects($"{@namespace}.{name}", folder, "src", settings);

            var file = ProjectsFile(projects);

            File.WriteAllText($"{folder}\\{@namespace}.{name}.sln", file);

            File.Move("./Resourse/.gitignore", $"{folder}/.gitignore");

            Directory.Move("./Resourse/shared", $"{folder}/shared");
            Directory.Move("./Resourse/lint", $"{folder}/lint");
            
        }

        private IEnumerable<ProjectInfoItem> GenerateProjects(string name, string folder, string path, SolutionSettings settings)
        {
            var result = new List<ProjectInfoItem>();
            var fullPath = $"{folder}\\{path}";

            var fmain = new ProjectInfoItem("Main", $"Main", ProjectType.Folder);
            var ftest = new ProjectInfoItem("Tests", $"Tests", ProjectType.Folder);
            var fshared = new ProjectInfoItem("Shared", $"Shared", ProjectType.Folder);

            var fconfigs = new ProjectInfoItem("configs", $"configs", ProjectType.Folder);
            fconfigs.Description =
    @"	ProjectSection(SolutionItems) = preProject
		shared\configs\connectionStrings.Debug.json = shared\configs\connectionStrings.Debug.json
		shared\configs\connectionStrings.Live.json = shared\configs\connectionStrings.Live.json
		shared\configs\connectionStrings.Release.json = shared\configs\connectionStrings.Release.json
		shared\configs\connectionStrings.Uat.json = shared\configs\connectionStrings.Uat.json
	EndProjectSection";

            var p1 = new ProjectGenerator($"{name}.Common", fullPath);
            p1.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Core", bgTeamVersion) });
            p1.Folder("Impl");
            p1.ClassTemplateFile("ITestService", "Common\\ITestService");
            p1.ClassTemplateFile("TestService", "Common\\TestService", new[] { "Impl" });
            var fp1 = new ProjectInfoItem(p1.Name, $"{path}\\{p1.Path}");

            var p2 = new ProjectGenerator($"{name}.DataAccess", fullPath);
            p2.ProjectFile(new NugetItem[] 
                {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.DataAccess", bgTeamVersion)
                }, new string[] { $"{name}.Domain" });
            p2.Folder("Impl");
            p2.ClassTemplateFile("TestQuery", "DataAccess\\TestQuery", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", name) });
            p2.ClassTemplateFile("TestQueryContext", "DataAccess\\TestQueryContext");
            var fp2 = new ProjectInfoItem(p2.Name, $"{path}\\{p2.Path}");

            var p3 = new ProjectGenerator($"{name}.Domain", fullPath);
            p3.ProjectFile(new NugetItem[] { new NugetItem("bgTeam.Impl.Dapper", bgTeamVersion) });
            p3.Folder("Dto");
            p3.Folder("Entities");
            p3.ClassTemplateFile("IEntity", "Domain\\IEntity");
            p3.ClassTemplateFile("Test", "Domain\\Test", new[] { "Entities" });
            p3.ClassTemplateFile("TestDto", "Domain\\TestDto", new[] { "Dto" });
            var fp3 = new ProjectInfoItem(p3.Name, $"{path}\\{p3.Path}");

            var p4 = new ProjectGenerator($"{name}.Story", fullPath);
            p4.ProjectFile(new NugetItem[]
                {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.DataAccess", bgTeamVersion)
                }, new string[] { $"{name}.Domain" });
            p4.ClassTemplateFile("TestStory", "Story\\TestStory", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", name) });
            p4.ClassTemplateFile("TestStoryContext", "Story\\TestStoryContext");
            p4.ClassTemplateFile("IStoryLibrary", "Story\\IStoryLibrary");
            p4.ClassTemplateFile("AutoMapperStory", "Story\\AutoMapperStory");
            var fp4 = new ProjectInfoItem(p4.Name, $"{path}\\{p4.Path}");

            var p6 = new ProjectGenerator($"{name}.Tests", $"{folder}\\tests");
            p6.ProjectFile(new NugetItem[]
                {
                    new NugetItem("bgTeam.Impl.MsSql", bgTeamVersion),
                    new NugetItem("Microsoft.NET.Test.Sdk", "15.5.0"),
                    new NugetItem("xunit", "2.3.1"),
                    new NugetItem("xunit.runner.visualstudio", "2.3.1")
                }, new string[] { $"{name}.Story" }, true);
            p6.Folder("Common");
            p6.ClassTemplateFile("TestStoryTests", "Tests\\TestStoryTests", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", name) });
            p6.ClassTemplateFile("FactoryTestService", "Tests\\FactoryTestService", new[] { "Common" });
            p6.JsonTemplateFile("appsettings", "Tests\\appsettings");
            var fp6 = new ProjectInfoItem(p6.Name, $"tests\\{p6.Path}", ProjectType.Compile);

            fmain.AddChild(fp1);
            fmain.AddChild(fp2);
            fmain.AddChild(fp3);
            fmain.AddChild(fp4);
            ftest.AddChild(fp6);
            fshared.AddChild(fconfigs);

            result.Add(fmain);
            result.Add(ftest);
            result.Add(fshared);
            result.Add(fconfigs);
            result.Add(fp1);
            result.Add(fp2);
            result.Add(fp3);
            result.Add(fp4);
            result.Add(fp6);

            if (settings.IsApp)
            {
                var p5 = new ProjectGenerator($"{name}.App", fullPath);
                p5.ProjectFile(new NugetItem[]
                    {
                    new NugetItem("bgTeam.Core", bgTeamVersion),
                    new NugetItem("bgTeam.Impl.Dapper", bgTeamVersion),
                    new NugetItem("bgTeam.Impl.MsSql", bgTeamVersion),
                    new NugetItem("Scrutor", "2.1.2")
                    }, new string[] { $"{name}.Story" }, true);
                p5.ClassTemplateFile("AppSettings", "App\\AppSettings");
                p5.ClassTemplateFile("AppIocConfigure", "App\\AppIocConfigure");
                p5.ClassTemplateFile("Program", "App\\Program", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", name) });
                p5.ClassTemplateFile("Runner", "App\\Runner", null, new List<KeyValueStr>() { new KeyValueStr("$prj$", name) });
                p5.JsonTemplateFile("appsettings", "App\\appsettings");
                var fp5 = new ProjectInfoItem(p5.Name, $"{path}\\{p5.Path}");

                fmain.AddChild(fp5);
                result.Add(fp5);
            }

            if (settings.IsWeb)
            {

            }

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

            foreach (var prj in projects)
            {
                str.AppendLine($"Project(\"{{{prj.Type.GetDescription()}}}\") = \"{prj.Name}\", \"{prj.Path}\", \"{{{prj.Code}}}\"");

                if (!string.IsNullOrEmpty(prj.Description))
                {
                    str.AppendLine(prj.Description);
                }

                str.AppendLine("EndProject");
            }

            //str.AppendLine()

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
                if (!prj.Build)
                {
                    continue;
                }

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
	EndGlobalSection");

            str.AppendLine("	GlobalSection(NestedProjects) = preSolution");
            foreach (var prj in projects)
            {
                foreach (var item in prj.ListChild)
                {
                    str.AppendLine($"		{{{item}}} = {{{prj.Code}}}");
                }
            }
            str.AppendLine("	EndGlobalSection");

            str.AppendLine(
@"    GlobalSection(ExtensibilityGlobals) = postSolution
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

        public bool Build { get; set; }

        public ProjectType Type { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public ProjectType Parent { get; private set; }

        public IList<string> ListChild { get; private set; }

        public ProjectInfoItem(string name, string path, ProjectType type = ProjectType.Compile)
        {
            Name = name;
            Path = path;
            Type = type;

            if (type == ProjectType.Compile || type == ProjectType.Tests)
            {
                Build = true;
            }

            Code = Guid.NewGuid().ToString().ToUpper();

            ListChild = new List<string>();
        }

        public void AddChild(ProjectInfoItem project)
        {
            ListChild.Add(project.Code.ToString());
        }
    }

    enum ProjectType
    {
        [Description("9A19103F-16F7-4668-BE54-9A1E7A4F7556")]
        Compile,

        [Description("2150E333-8FDC-42A3-9474-1A3956D46DE8")]
        Folder,

        [Description("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC")]
        Tests,

    }

    public class SolutionSettings
    {
        public bool IsWeb { get; internal set; }
        public bool IsApp { get; internal set; }
    }
}
