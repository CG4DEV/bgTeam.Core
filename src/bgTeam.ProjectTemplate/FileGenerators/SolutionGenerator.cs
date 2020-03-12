namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using bgTeam.Extensions;

    internal class SolutionGenerator
    {
        public void Generate(string @namespace, string name, SolutionSettings settings)
        {
            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "result");

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            var projects = GenerateProjects($"{@namespace}.{name}", folder, "src", settings);

            var file = ProjectsFile(projects);

            File.WriteAllText($"{folder}{Path.DirectorySeparatorChar}{@namespace}.{name}.sln", file);

            File.Copy("./Resourse/.gitignore", $"{folder}/.gitignore");

            foreach (var item in Directory.EnumerateFiles("./Resourse/shared", "*", SearchOption.AllDirectories))
            {
                var dest = $"{folder}/{Path.GetRelativePath("./Resourse/", item)}";
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(item, dest);
            }

            foreach (var item in Directory.EnumerateFiles("./Resourse/lint", "*", SearchOption.AllDirectories))
            {
                var dest = $"{folder}/{Path.GetRelativePath("./Resourse/", item)}";
                Directory.CreateDirectory(Path.GetDirectoryName(dest));
                File.Copy(item, dest);
            }
        }

        private IEnumerable<ProjectInfoItem> GenerateProjects(string name, string folder, string path, SolutionSettings settings)
        {
            var result = new List<ProjectInfoItem>();
            var fullPath = $"{folder}{Path.DirectorySeparatorChar}{path}";

            var fmain = new ProjectInfoItem("Main", $"Main", ProjectType.Folder);
            var ftest = new ProjectInfoItem("Tests", $"Tests", ProjectType.Folder);
            var fshared = new ProjectInfoItem("Shared", $"Shared", ProjectType.Folder);

            var fconfigs = new ProjectInfoItem("configs", $"configs", ProjectType.Folder)
            {
                Description =
    @"	ProjectSection(SolutionItems) = preProject
		shared\configs\connectionStrings.Debug.json = shared\configs\connectionStrings.Debug.json
		shared\configs\connectionStrings.Production.json = shared\configs\connectionStrings.Production.json
		shared\configs\connectionStrings.Release.json = shared\configs\connectionStrings.Release.json
		shared\configs\connectionStrings.Uat.json = shared\configs\connectionStrings.Uat.json
	EndProjectSection",
            };

            var p1 = new ProjectGenerator($"{name}.Common", fullPath);
            p1.ProjectFile(new[] { ("bgTeam.Core", settings.BgTeamVersion) });
            p1.Folder("Impl");
            p1.ClassTemplateFile("ITestService", $"Common{Path.DirectorySeparatorChar}ITestService");
            p1.ClassTemplateFile("TestService", $"Common{Path.DirectorySeparatorChar}TestService", new[] { "Impl" });

            if (settings.IsWeb)
            {
                p1.Folder("Exceptions");
                p1.ClassTemplateFile("BadRequestException", $"Common{Path.DirectorySeparatorChar}BadRequestException", new[] { "Exceptions" });
            }

            var fp1 = new ProjectInfoItem(p1.Name, $"{path}{Path.DirectorySeparatorChar}{p1.Output}");

            var p2 = new ProjectGenerator($"{name}.DataAccess", fullPath);
            p2.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.DataAccess", settings.BgTeamVersion),
                }, projects: new[] { $"{name}.Domain" });
            p2.Folder("Impl");
            p2.ClassTemplateFile("TestQuery", $"DataAccess{Path.DirectorySeparatorChar}TestQuery", replist: new List<(string, string)> { ("$prj$", name) });
            p2.ClassTemplateFile("TestQueryContext", $"DataAccess{Path.DirectorySeparatorChar}TestQueryContext");
            var fp2 = new ProjectInfoItem(p2.Name, $"{path}{Path.DirectorySeparatorChar}{p2.Output}");

            var p3 = new ProjectGenerator($"{name}.Domain", fullPath);
            p3.ProjectFile(new[] { ("bgTeam.Impl.Dapper", settings.BgTeamVersion) });
            p3.Folder("Dto");
            p3.Folder("Entities");
            p3.ClassTemplateFile("IEntity", $"Domain{Path.DirectorySeparatorChar}IEntity");
            p3.ClassTemplateFile("Test", $"Domain{Path.DirectorySeparatorChar}Test", new[] { "Entities" });
            p3.ClassTemplateFile("TestDto", $"Domain{Path.DirectorySeparatorChar}TestDto", new[] { "Dto" });
            var fp3 = new ProjectInfoItem(p3.Name, $"{path}{Path.DirectorySeparatorChar}{p3.Output}");

            var p4 = new ProjectGenerator($"{name}.Story", fullPath);
            p4.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.DataAccess", settings.BgTeamVersion),
                }, projects: new[] { $"{name}.Domain" });
            p4.ClassTemplateFile("TestStory", $"Story{Path.DirectorySeparatorChar}TestStory", replist: new List<(string, string)> { ("$prj$", name) });
            p4.ClassTemplateFile("TestStoryContext", $"Story{Path.DirectorySeparatorChar}TestStoryContext");
            p4.ClassTemplateFile("IStoryLibrary", $"Story{Path.DirectorySeparatorChar}IStoryLibrary");
            var fp4 = new ProjectInfoItem(p4.Name, $"{path}{Path.DirectorySeparatorChar}{p4.Output}");

            var p5 = new ProjectGenerator($"{name}.Tests", $"{folder}{Path.DirectorySeparatorChar}tests");
            p5.ProjectFile(
                new[]
                {
                    ("bgTeam.Impl.MsSql", settings.BgTeamVersion),
                    ("Microsoft.NET.Test.Sdk", "16.2.0"),
                    ("xunit", "2.4.0"),
                    ("xunit.runner.visualstudio", "2.4.0"),
                }, projects: new[] { $"{name}.Story" },
                configs: true);
            p5.Folder("Common");
            p5.ClassTemplateFile("TestStoryTests", $"Tests{Path.DirectorySeparatorChar}TestStoryTests", replist: new List<(string, string)> { ("$prj$", name) });
            p5.ClassTemplateFile("FactoryTestService", $"Tests{Path.DirectorySeparatorChar}FactoryTestService", new[] { "Common" });
            p5.JsonTemplateFile("appsettings", $"Tests{Path.DirectorySeparatorChar}appsettings");
            var fp5 = new ProjectInfoItem(p5.Name, $"tests{Path.DirectorySeparatorChar}{p5.Output}", ProjectType.Compile);

            fmain.AddChild(fp1);
            fmain.AddChild(fp2);
            fmain.AddChild(fp3);
            fmain.AddChild(fp4);
            ftest.AddChild(fp5);
            fshared.AddChild(fconfigs);

            result.Add(fmain);
            result.Add(ftest);
            result.Add(fshared);
            result.Add(fconfigs);
            result.Add(fp1);
            result.Add(fp2);
            result.Add(fp3);
            result.Add(fp4);
            result.Add(fp5);

            if (settings.IsApp)
            {
                var p6 = new ProjectGenerator($"{name}.App", fullPath);
                p6.ProjectFile(
                    new[]
                    {
                        ("bgTeam.Core", settings.BgTeamVersion),
                        ("bgTeam.Impl.Dapper", settings.BgTeamVersion),
                        ("bgTeam.Impl.MsSql", settings.BgTeamVersion),
                        ("Scrutor", "2.1.2"),
                    }, type: "Exe",
                    projects: new[] { $"{name}.Story" },
                    configs: true);
                p6.ClassTemplateFile("AppSettings", $"App{Path.DirectorySeparatorChar}AppSettings");
                p6.ClassTemplateFile("AppIocConfigure", $"App{Path.DirectorySeparatorChar}AppIocConfigure", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("Program", $"App{Path.DirectorySeparatorChar}Program", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("Application", $"App{Path.DirectorySeparatorChar}Application", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("ApplicationBuilder", $"App{Path.DirectorySeparatorChar}ApplicationBuilder", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("Runner", $"App{Path.DirectorySeparatorChar}Runner", replist: new List<(string, string)> { ("$prj$", name) });
                p6.JsonTemplateFile("appsettings", $"App{Path.DirectorySeparatorChar}appsettings");
                p6.Folder("Properties");
                p6.JsonTemplateFile("launchSettings", $"App{Path.DirectorySeparatorChar}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p6.Name) });
                var fp6 = new ProjectInfoItem(p6.Name, $"{path}{Path.DirectorySeparatorChar}{p6.Output}");
                fmain.AddChild(fp6);
                result.Add(fp6);
            }

            if (settings.IsWeb)
            {
                var p7 = new ProjectGenerator($"{name}.WebApp", fullPath);
                p7.ProjectFile(
                    new[]
                    {
                        ("bgTeam.Core", settings.BgTeamVersion),
                        ("bgTeam.Impl.Dapper", settings.BgTeamVersion),
                        ("bgTeam.Impl.MsSql", settings.BgTeamVersion),
                        ("Microsoft.AspNetCore.App", "3.1.0"),
                        ("Scrutor", "2.1.2"),
                    }, projects: new[] { $"{name}.Common", $"{name}.DataAccess", $"{name}.Story" });
                p7.ClassTemplateFile("AppSettings", $"App{Path.DirectorySeparatorChar}AppSettings");
                p7.ClassTemplateFile("AppIocConfigure", $"WebApp{Path.DirectorySeparatorChar}AppIocConfigure", replist: new List<(string, string)> { ("$prj$", name) });
                p7.ClassTemplateFile("AppMiddlewareException", $"WebApp{Path.DirectorySeparatorChar}AppMiddlewareException", replist: new List<(string, string)> { ("$prj$", name) });
                p7.Folder("Controllers");
                p7.ClassTemplateFile("HomeController", $"WebApp{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}HomeController", new[] { "Controllers" });
                p7.ClassTemplateFile("ApiController", $"WebApp{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}ApiController", new[] { "Controllers" });
                var fp7 = new ProjectInfoItem(p7.Name, $"{path}{Path.DirectorySeparatorChar}{p7.Output}");
                fmain.AddChild(fp7);
                result.Add(fp7);

                var p8 = new ProjectGenerator($"{name}.Web", fullPath);
                p8.ProjectFile(
                    new[]
                    {
                        ("Swashbuckle.AspNetCore", "5.0.0"),
                        ("Microsoft.AspNetCore.Mvc.NewtonsoftJson", "3.1.0"),
                    },
                    "Microsoft.NET.Sdk.Web",
                    "Exe",
                    new[] { $"{name}.WebApp" },
                    true);
                p8.ClassTemplateFile("Program", $"Web{Path.DirectorySeparatorChar}Program", replist: new List<(string, string)> { ("$prj$", name) });
                p8.ClassTemplateFile("Startup", $"Web{Path.DirectorySeparatorChar}Startup", replist: new List<(string, string)> { ("$prj$", name), ("$api-name$", $"{name} API") });
                p8.JsonTemplateFile("appsettings", $"Web{Path.DirectorySeparatorChar}appsettings");
                p8.Folder("Properties");
                p8.JsonTemplateFile("launchSettings", $"Web{Path.DirectorySeparatorChar}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p8.Name) });

                var fp8 = new ProjectInfoItem(p8.Name, $"{path}{Path.DirectorySeparatorChar}{p8.Output}");
                fmain.AddChild(fp8);
                result.Add(fp8);
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

            Code = Guid.NewGuid().ToString().ToUpperInvariant();

            ListChild = new List<string>();
        }

        public void AddChild(ProjectInfoItem project)
        {
            ListChild.Add(project.Code);
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

        public string BgTeamVersion { get; set; }
    }
}
