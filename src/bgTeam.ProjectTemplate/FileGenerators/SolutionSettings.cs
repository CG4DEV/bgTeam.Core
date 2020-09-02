namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using bgTeam.Extensions;

    public class SolutionSettings
    {
        public bool IsWeb { get; internal set; }

        public bool IsApp { get; internal set; }

        public string BgTeamVersion { get; set; }
    }
}
