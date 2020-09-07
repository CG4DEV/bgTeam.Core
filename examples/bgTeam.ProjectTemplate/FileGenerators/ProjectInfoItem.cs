namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;

    internal class ProjectInfoItem
    {
        private string _path;

        public string Name { get; set; }

        public string Path
        {
            get
            {
                return _path.Replace('/', '\\');
            }
            set
            {
                _path = value;
            }
        }

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
