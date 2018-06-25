using System;

namespace bgTeam.ProjectTemplate
{
    class Program
    {
        static void Main(string[] args)
        {
            var s1 = new SolutionGenerator();

            s1.Generate("Trcont", "FilesStorage", new SolutionSettings() { IsWeb = true, IsApp = true });
        }
    }
}
