using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace bgTeam.ProjectTemplate
{
    public class ProjectGenerator
    {
        private string _resPath;

        public string Name { get; set; }

        public string Path { get; private set; }

        public ProjectGenerator(string name, string resPath)
        {
            Name = name;
            _resPath = resPath;

            var dir = $"{_resPath}\\{Name}";

            if (Directory.Exists(dir))
            {
                Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
            else
            {
                Directory.CreateDirectory(dir);
            }
        }

        public void ProjectFile(NugetItem[] nugets, string[] projects = null, bool configs = false)
        {
            string temp = File.ReadAllText("./Resourse/Templates/project.txt");

            var strNuget = new StringBuilder();
            strNuget.AppendLine("  <ItemGroup>");
            foreach (var item in nugets)
            {
                strNuget.AppendLine($"    <PackageReference Include=\"{item.Name}\" Version=\"{item.Version}\" />");
            }
            strNuget.AppendLine("  </ItemGroup>");

            temp = temp.Replace("$nugets$", strNuget.ToString());

            if (projects != null)
            {
                var strPrj = new StringBuilder();
                strPrj.AppendLine("  <ItemGroup>");
                foreach (var item in projects)
                {
                    strPrj.AppendLine($"    <ProjectReference Include=\"..\\..\\src\\{item}\\{item}.csproj\" />");
                }
                strPrj.AppendLine("  </ItemGroup>");

                temp = temp.Replace("$projects$", strPrj.ToString());
            }
            else
            {
                temp = temp.Replace("$projects$", string.Empty);
            }

            if (configs)
            {
                string tempCnf = File.ReadAllText("./Resourse/Templates/configs.txt");

                temp = temp.Replace("$configs$", tempCnf);
            }
            else
            {
                temp = temp.Replace("$configs$", string.Empty);
            }

            Path = $"{Name}\\{Name}.csproj";

            File.WriteAllText($"{_resPath}\\{Path}", temp);
        }

        internal void ClassTemplateFile(string name, string pathTemp, string[] folderPath = null, IList<KeyValueStr> replist = null)
        {
            if (folderPath == null)
            {
                folderPath = new string[] { };
            }

            string temp = File.ReadAllText($".\\Resourse\\TemplatesClass\\{pathTemp}.txt");

            var pathList = new List<string>();

            pathList.Add(Name);
            pathList.AddRange(folderPath);

            //temp = temp.Replace("$name$", name);
            temp = temp.Replace("$namespace$", string.Join('.', pathList));

            if (replist != null && replist.Count > 0)
            {
                foreach (var item in replist)
                {
                    temp = temp.Replace(item.Key, item.Value);
                }
            }

            File.WriteAllText($"{_resPath}\\{Name}\\{string.Join('\\', folderPath)}\\{name}.cs", temp);
        }

        internal void JsonTemplateFile(string name, string pathTemp, params string[] folderPath)
        {
            string temp = File.ReadAllText($".\\Resourse\\TemplatesClass\\{pathTemp}.json");

            //var pathList = new List<string>();

            //pathList.Add(Name);
            //pathList.AddRange(folderPath);

            //temp = temp.Replace("$namespace$", string.Join('.', pathList));

            File.WriteAllText($"{_resPath}\\{Name}\\{string.Join('\\', folderPath)}\\{name}.json", temp);
        }

        public void Folder(string folder)
        {
            Directory.CreateDirectory($"{_resPath}\\{Name}\\{folder}");
        }
    }

    public class NugetItem
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public NugetItem(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }

    public class KeyValueStr
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public KeyValueStr(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
