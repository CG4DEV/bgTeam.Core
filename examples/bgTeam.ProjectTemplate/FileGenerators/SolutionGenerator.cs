namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

            var nameSln = string.Join('.', new[] { @namespace, name }.Where(x => x != null));

            var projects = GenerateProjects(nameSln, folder, "src", settings);

            var file = ProjectsFile(projects);

            File.WriteAllText($"{folder}{Path.DirectorySeparatorChar}{nameSln}.sln", file);

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

            foreach (var item in Directory.EnumerateFiles("./Resourse/wiki-generator", "*", SearchOption.AllDirectories))
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

            var fapps = new ProjectInfoItem("Apps", $"Apps", ProjectTypeEnum.Folder);
            var fmain = new ProjectInfoItem("Main", $"Main", ProjectTypeEnum.Folder);
            var fshared = new ProjectInfoItem("Shared", $"Shared", ProjectTypeEnum.Folder);
            var fsrv = new ProjectInfoItem("Services", $"Shared", ProjectTypeEnum.Folder);
            var ftest = new ProjectInfoItem("Tests", $"Tests", ProjectTypeEnum.Folder);

            var fconfigs = new ProjectInfoItem("configs", $"configs", ProjectTypeEnum.Folder)
            {
                Description =
    @"	ProjectSection(SolutionItems) = preProject
		shared\configs\connectionStrings.Development.json = shared\configs\connectionStrings.Development.json
		shared\configs\connectionStrings.Production.json = shared\configs\connectionStrings.Production.json
        shared\configs\serilog.Development.json = shared\configs\serilog.Development.json
		shared\configs\serilog.Production.json = shared\configs\serilog.Production.json
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
                    ("Microsoft.NET.Test.Sdk", "16.7.1"),
                    ("xunit", "2.4.1"),
                    ("xunit.runner.visualstudio", "2.4.3"),
                }, projects: new[] { $"{name}.Story" },
                configs: true);
            p5.Folder("Common");
            p5.ClassTemplateFile("TestStoryTests", $"Tests{Path.DirectorySeparatorChar}TestStoryTests", replist: new List<(string, string)> { ("$prj$", name) });
            p5.ClassTemplateFile("FactoryTestService", $"Tests{Path.DirectorySeparatorChar}FactoryTestService", new[] { "Common" });
            p5.JsonTemplateFile("appsettings", $"Tests{Path.DirectorySeparatorChar}appsettings");
            p5.JsonTemplateFile("appsettings.Development", $"Web{Path.DirectorySeparatorChar}appsettings");
            p5.JsonTemplateFile("appsettings.Production", $"Web{Path.DirectorySeparatorChar}appsettings");
            var fp5 = new ProjectInfoItem(p5.Name, $"tests{Path.DirectorySeparatorChar}{p5.Output}", ProjectTypeEnum.Compile);

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
                        ("Scrutor", "3.2.2"),
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
                p6.JsonTemplateFile("appsettings.Development", $"Web{Path.DirectorySeparatorChar}appsettings");
                p6.JsonTemplateFile("appsettings.Production", $"Web{Path.DirectorySeparatorChar}appsettings");
                p6.Folder("Properties");
                p6.JsonTemplateFile("launchSettings", $"App{Path.DirectorySeparatorChar}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p6.Name) });
                var fp6 = new ProjectInfoItem(p6.Name, $"{path}{Path.DirectorySeparatorChar}{p6.Output}");
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
                p7.ClassTemplateFile("AppSettings", $"App{Path.DirectorySeparatorChar}AppSettings");
                p7.ClassTemplateFile("AppIocConfigure", $"WebApp{Path.DirectorySeparatorChar}AppIocConfigure", replist: new List<(string, string)> { ("$prj$", name) });
                p7.ClassTemplateFile("AppMiddlewareException", $"WebApp{Path.DirectorySeparatorChar}AppMiddlewareException", replist: new List<(string, string)> { ("$prj$", name) });
                p7.Folder("Controllers");
                p7.ClassTemplateFile("HomeController", $"WebApp{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}HomeController", new[] { "Controllers" }, new List<(string, string)> { ("$api-name$", $"{name} API") });
                p7.ClassTemplateFile("UserController", $"WebApp{Path.DirectorySeparatorChar}Controllers{Path.DirectorySeparatorChar}UserController", new[] { "Controllers" });
                var fp7 = new ProjectInfoItem(p7.Name, $"{path}{Path.DirectorySeparatorChar}{p7.Output}");
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
                p8.ClassTemplateFile("Program", $"Web{Path.DirectorySeparatorChar}Program", replist: new List<(string, string)> { ("$prj$", name) });
                p8.ClassTemplateFile("Startup", $"Web{Path.DirectorySeparatorChar}Startup", replist: new List<(string, string)> { ("$prj$", name), ("$api-name$", $"{name} API") });
                p8.JsonTemplateFile("appsettings", $"Web{Path.DirectorySeparatorChar}appsettings");
                p8.JsonTemplateFile("appsettings.Development", $"Web{Path.DirectorySeparatorChar}appsettings");
                p8.JsonTemplateFile("appsettings.Production", $"Web{Path.DirectorySeparatorChar}appsettings");
                p8.Folder("Properties");
                p8.JsonTemplateFile("launchSettings", $"Web{Path.DirectorySeparatorChar}launchSettings", new[] { "Properties" }, new List<(string, string)> { ("$prj$", p8.Name) });

                var fp8 = new ProjectInfoItem(p8.Name, $"{path}{Path.DirectorySeparatorChar}{p8.Output}");
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
            p9.ClassTemplateFile("AppIocConfigure", $"StoryRunner{Path.DirectorySeparatorChar}AppIocConfigure");
            p9.ClassTemplateFile("AppSettings", $"StoryRunner{Path.DirectorySeparatorChar}AppSettings");
            p9.ClassTemplateFile("Program", $"StoryRunner{Path.DirectorySeparatorChar}Program");
            p9.ClassTemplateFile("Runner", $"StoryRunner{Path.DirectorySeparatorChar}Runner");
            p9.JsonTemplateFile("appsettings", $"StoryRunner{Path.DirectorySeparatorChar}appsettings");
            p9.JsonTemplateFile("appsettings.Development", $"StoryRunner{Path.DirectorySeparatorChar}appsettings");
            p9.JsonTemplateFile("appsettings.Production", $"StoryRunner{Path.DirectorySeparatorChar}appsettings");
            var fp9 = new ProjectInfoItem(p9.Name, $"{path}{Path.DirectorySeparatorChar}{p9.Output}");
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
            p10.ClassTemplateFile("AppIocConfigure", $"StoryScheduler{Path.DirectorySeparatorChar}AppIocConfigure");
            p10.ClassTemplateFile("AppSettings", $"StoryScheduler{Path.DirectorySeparatorChar}AppSettings");
            p10.ClassTemplateFile("Program", $"StoryScheduler{Path.DirectorySeparatorChar}Program");
            p10.ClassTemplateFile("Runner", $"StoryScheduler{Path.DirectorySeparatorChar}Runner");
            p10.JsonTemplateFile("appsettings", $"StoryScheduler{Path.DirectorySeparatorChar}appsettings");
            p10.JsonTemplateFile("appsettings.Development", $"StoryScheduler{Path.DirectorySeparatorChar}appsettings");
            p10.JsonTemplateFile("appsettings.Production", $"StoryScheduler{Path.DirectorySeparatorChar}appsettings");
            p10.Folder("Configurations");
            p10.JsonTemplateFile("0_Examples", $"StoryScheduler{Path.DirectorySeparatorChar}0_Examples", new[] { "Configurations" });
            var fp10 = new ProjectInfoItem(p10.Name, $"{path}{Path.DirectorySeparatorChar}{p10.Output}");
            fsrv.AddChild(fp10);
            result.Add(fp10);

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
}
