namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using bgTeam.Extensions;

    internal class ProjectInfoItem
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool Build { get; set; }

        public ProjectTypeEnum Type { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public ProjectTypeEnum Parent { get; private set; }

        public IList<string> ListChild { get; private set; }

        public ProjectInfoItem(string name, string path, ProjectTypeEnum type = ProjectTypeEnum.Compile)
        {
            Name = name;
            Path = path;
            Type = type;

            if (type == ProjectTypeEnum.Compile || type == ProjectTypeEnum.Tests)
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
}
