namespace bgTeam.ProjectTemplate
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using bgTeam.Extensions;

    internal class SolutionGenerator
    {
        private readonly char _separator = Path.DirectorySeparatorChar;

        public void Generate(string @namespace, string name, SolutionSettings settings)
        {
            var folder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "result");

            if (Directory.Exists(folder))
            {
                Directory.Delete(folder, true);
            }

            var nameSln = string.Join('.', new[] { @namespace, name }.Where(x => x != null));

            var projects = GenerateProjects(nameSln, folder, "src", settings);

            var file = ProjectsFile(projects);

            File.WriteAllText($"{folder}{_separator}{nameSln}.sln", file);

            File.Copy("./Resourse/LICENSE", $"{folder}/LICENSE");
            File.Copy("./Resourse/README.md", $"{folder}/README.md");
            File.Copy("./Resourse/.gitignore", $"{folder}/.gitignore");
            File.WriteAllText($"{folder}/coverage.bat", File.ReadAllText("./Resourse/coverage.bat").Replace("$namespace$", @namespace));

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
            var fullPath = $"{folder}{_separator}{path}";

            var fapps = new ProjectInfoItem("Apps", $"Apps", ProjectTypeEnum.Folder);
            var fmain = new ProjectInfoItem("Main", $"Main", ProjectTypeEnum.Folder);
            var fshared = new ProjectInfoItem("Shared", $"Shared", ProjectTypeEnum.Folder);
            var fsrv = new ProjectInfoItem("Services", $"Services", ProjectTypeEnum.Folder);
            var ftest = new ProjectInfoItem("Tests", $"Tests", ProjectTypeEnum.Folder);

            var fconfigs = new ProjectInfoItem("configs", $"configs", ProjectTypeEnum.Folder)
            {
                Description =
    @"ProjectSection(SolutionItems) = preProject
		shared\configs\connectionStrings.Development.json = shared\configs\connectionStrings.Development.json
		shared\configs\connectionStrings.Production.json = shared\configs\connectionStrings.Production.json
		shared\configs\serilog.Development.json = shared\configs\serilog.Development.json
		shared\configs\serilog.Production.json = shared\configs\serilog.Production.json
	EndProjectSection",
            };

            var p1 = new ProjectGenerator($"{name}.Common", fullPath);
            p1.ProjectFile(new[] { ("bgTeam.Core", settings.BgTeamVersion) });
            p1.Folder("Impl");
            p1.ClassTemplateFile("ITestService", $"Common{_separator}ITestService");
            p1.ClassTemplateFile("TestService", $"Common{_separator}Impl{_separator}TestService", new[] { "Impl" });

            if (settings.IsWeb)
            {
                p1.Folder("Exceptions");
                p1.ClassTemplateFile("BadRequestException", $"Common{_separator}BadRequestException", new[] { "Exceptions" });
            }

            var fp1 = new ProjectInfoItem(p1.Name, $"{path}{_separator}{p1.Output}");

            var p2 = new ProjectGenerator($"{name}.DataAccess", fullPath);
            p2.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.DataAccess", settings.BgTeamVersion),
                    ("Microsoft.EntityFrameworkCore", settings.EntityFrameworkCoreVersion),
                    ("Microsoft.EntityFrameworkCore.Proxies", settings.EntityFrameworkCoreVersion),
                    ("Npgsql.EntityFrameworkCore.PostgreSQL", settings.EntityFrameworkCoreNpgsqlVersion),
                }, projects: new[] { $"{name}.Domain" });
            p2.Folder("Impl");
            p2.ClassTemplateFile("TestQuery", $"DataAccess{_separator}TestQuery", replist: new List<(string, string)> { ("$prj$", name) });
            p2.ClassTemplateFile("TestQueryContext", $"DataAccess{_separator}TestQueryContext");
            var fp2 = new ProjectInfoItem(p2.Name, $"{path}{_separator}{p2.Output}");

            var p3 = new ProjectGenerator($"{name}.Domain", fullPath);
            p3.ProjectFile(new[] { ("bgTeam.Impl.Dapper", settings.BgTeamVersion) });
            p3.Folder("Dto");
            p3.Folder("Entities");
            p3.ClassTemplateFile("IEntity", $"Domain{_separator}IEntity");
            p3.ClassTemplateFile("Test", $"Domain{_separator}Test", new[] { "Entities" });
            p3.ClassTemplateFile("TestDto", $"Domain{_separator}Dto{_separator}TestDto", new[] { "Dto" });
            var fp3 = new ProjectInfoItem(p3.Name, $"{path}{_separator}{p3.Output}");

            var p4 = new ProjectGenerator($"{name}.Story", fullPath);
            p4.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.DataAccess", settings.BgTeamVersion),
                }, projects: new[] { $"{name}.Domain" });
            p4.ClassTemplateFile("TestStory", $"Story{_separator}TestStory", replist: new List<(string, string)> { ("$prj$", name) });
            p4.ClassTemplateFile("TestStoryContext", $"Story{_separator}TestStoryContext");
            p4.ClassTemplateFile("IStoryLibrary", $"Story{_separator}IStoryLibrary");
            var fp4 = new ProjectInfoItem(p4.Name, $"{path}{_separator}{p4.Output}");

            var p5 = new ProjectGenerator($"{name}.Tests", $"{folder}{_separator}tests");
            p5.ProjectFile(
                new[]
                {
                    ("bgTeam.Impl.MsSql", settings.BgTeamVersion),
                    ("Microsoft.NET.Test.Sdk", "16.7.1"),
                    ("xunit", "2.4.1"),
                    ("xunit.runner.visualstudio", "2.4.3"),
                }, projects: new[] { $"{name}.Story" },
                configs: true);
            p5.Folder("Common");
            p5.ClassTemplateFile("TestStoryTests", $"Tests{_separator}TestStoryTests", replist: new List<(string, string)> { ("$prj$", name) });
            p5.ClassTemplateFile("FactoryTestService", $"Tests{_separator}FactoryTestService", new[] { "Common" });
            p5.JsonTemplateFile("appsettings", $"Tests{_separator}appsettings");
            p5.JsonTemplateFile("appsettings.Development", $"Web{_separator}appsettings");
            p5.JsonTemplateFile("appsettings.Production", $"Web{_separator}appsettings");
            var fp5 = new ProjectInfoItem(p5.Name, $"tests{_separator}{p5.Output}", ProjectTypeEnum.Compile);

            fmain.AddChild(fp1);
            fmain.AddChild(fp2);
            fmain.AddChild(fp3);
            fmain.AddChild(fp4);
            ftest.AddChild(fp5);
            fshared.AddChild(fconfigs);

            result.Add(fapps);
            result.Add(fmain);
            result.Add(ftest);
            result.Add(fshared);
            result.Add(fsrv);
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
                        ("bgTeam.Impl.Serilog", settings.BgTeamVersion),
                        ("Scrutor", "3.2.2"),
                    }, type: "Exe",
                    projects: new[] { $"{name}.Story" },
                    configs: true);
                p6.ClassTemplateFile("AppSettings", $"App{_separator}AppSettings");
                p6.ClassTemplateFile("AppIocConfigure", $"App{_separator}AppIocConfigure", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("Program", $"App{_separator}Program", replist: new List<(string, string)> { ("$prj$", name) });
                p6.ClassTemplateFile("Runner", $"App{_separator}Runner", replist: new List<(string, string)> { ("$prj$", name) });
                p6.JsonTemplateFile("appsettings", $"App{_separator}appsettings");
                p6.JsonTemplateFile("appsettings.Development", $"App{_separator}appsettings");
                p6.JsonTemplateFile("appsettings.Production", $"App{_separator}appsettings");
                p6.Folder("Properties");
                p6.JsonTemplateFile("launchSettings", $"App{_separator}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p6.Name) });
                var fp6 = new ProjectInfoItem(p6.Name, $"{path}{_separator}{p6.Output}");
                //fmain.AddChild(fp6);
                fapps.AddChild(fp6);
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
                        ("Microsoft.AspNetCore.App", "2.2.8"),
                        ("Scrutor", "3.2.2"),
                    }, projects: new[] { $"{name}.Common", $"{name}.DataAccess", $"{name}.Story" });
                p7.ClassTemplateFile("AppSettings", $"App{_separator}AppSettings");
                p7.ClassTemplateFile("AppIocConfigure", $"WebApp{_separator}AppIocConfigure", replist: new List<(string, string)> { ("$prj$", name) });
                p7.ClassTemplateFile("AppMiddlewareException", $"WebApp{_separator}AppMiddlewareException", replist: new List<(string, string)> { ("$prj$", name) });
                p7.Folder("Controllers");
                p7.ClassTemplateFile("HomeController", $"WebApp{_separator}Controllers{_separator}HomeController", new[] { "Controllers" }, new List<(string, string)> { ("$api-name$", $"{name} API") });
                p7.ClassTemplateFile("UserController", $"WebApp{_separator}Controllers{_separator}UserController", new[] { "Controllers" });
                var fp7 = new ProjectInfoItem(p7.Name, $"{path}{_separator}{p7.Output}");
                fmain.AddChild(fp7);
                result.Add(fp7);

                var p8 = new ProjectGenerator($"{name}.Web", fullPath);
                p8.ProjectFile(
                    new[]
                    {
                        ("Swashbuckle.AspNetCore", "5.5.1"),
                        ("Microsoft.AspNetCore.Mvc.NewtonsoftJson", "3.1.7"),
                    },
                    "Microsoft.NET.Sdk.Web",
                    "Exe",
                    new[] { $"{name}.WebApp" },
                    true);
                p8.ClassTemplateFile("Program", $"Web{_separator}Program", replist: new List<(string, string)> { ("$prj$", name) });
                p8.ClassTemplateFile("Startup", $"Web{_separator}Startup", replist: new List<(string, string)> { ("$prj$", name), ("$api-name$", $"{name} API") });
                p8.JsonTemplateFile("appsettings", $"Web{_separator}appsettings");
                p8.JsonTemplateFile("appsettings.Development", $"Web{_separator}appsettings");
                p8.JsonTemplateFile("appsettings.Production", $"Web{_separator}appsettings");
                p8.Folder("Properties");
                p8.JsonTemplateFile("launchSettings", $"Web{_separator}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p8.Name) });

                var fp8 = new ProjectInfoItem(p8.Name, $"{path}{_separator}{p8.Output}");
                //fmain.AddChild(fp8);
                fapps.AddChild(fp8);
                result.Add(fp8);
            }

            var p9 = new ProjectGenerator($"{name}.StRunner", fullPath);
            p9.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.Queues", settings.BgTeamVersion),
                    ("bgTeam.StoryRunner", settings.BgTeamVersion),
                    ("Microsoft.AspNetCore", "2.2.0"),
                    ("Scrutor", "3.2.2"),
                },
                type: "Exe",
                projects: new[] { $"{name}.Story" },
                configs: true);
            p9.ClassTemplateFile("AppIocConfigure", $"StoryRunner{_separator}AppIocConfigure");
            p9.ClassTemplateFile("AppSettings", $"StoryRunner{_separator}AppSettings");
            p9.ClassTemplateFile("Program", $"StoryRunner{_separator}Program");
            p9.ClassTemplateFile("Runner", $"StoryRunner{_separator}Runner");
            p9.JsonTemplateFile("appsettings", $"StoryRunner{_separator}appsettings");
            p9.JsonTemplateFile("appsettings.Development", $"StoryRunner{_separator}appsettings");
            p9.JsonTemplateFile("appsettings.Production", $"StoryRunner{_separator}appsettings");
            var fp9 = new ProjectInfoItem(p9.Name, $"{path}{_separator}{p9.Output}");
            fsrv.AddChild(fp9);
            result.Add(fp9);

            var p10 = new ProjectGenerator($"{name}.StScheduler", fullPath);
            p10.ProjectFile(
                new[]
                {
                    ("bgTeam.Core", settings.BgTeamVersion),
                    ("bgTeam.Queues", settings.BgTeamVersion),
                    ("bgTeam.DataAccess", settings.BgTeamVersion),
                    ("bgTeam.StoryRunnerScheduler", settings.BgTeamVersion),
                    ("Microsoft.AspNetCore", "2.2.0"),
                    ("Quartz", "3.1.0"),
                },
                type: "Exe",
                configs: true);
            p10.ClassTemplateFile("AppIocConfigure", $"StoryScheduler{_separator}AppIocConfigure");
            p10.ClassTemplateFile("AppSettings", $"StoryScheduler{_separator}AppSettings");
            p10.ClassTemplateFile("Program", $"StoryScheduler{_separator}Program");
            p10.ClassTemplateFile("Runner", $"StoryScheduler{_separator}Runner");
            p10.JsonTemplateFile("appsettings", $"StoryScheduler{_separator}appsettings");
            p10.JsonTemplateFile("appsettings.Development", $"StoryScheduler{_separator}appsettings");
            p10.JsonTemplateFile("appsettings.Production", $"StoryScheduler{_separator}appsettings");
            p10.Folder("Configurations");
            p10.JsonTemplateFile("0_Examples", $"StoryScheduler{_separator}0_Examples", new[] { "Configurations" });
            var fp10 = new ProjectInfoItem(p10.Name, $"{path}{_separator}{p10.Output}");
            fsrv.AddChild(fp10);
            result.Add(fp10);

            return result;
        }

        private string ProjectsFile(IEnumerable<ProjectInfoItem> projects)
        {
            var str = new StringBuilder();

            str.AppendLine(
@"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio Version 16
VisualStudioVersion = 16.0.30413.136
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
}
