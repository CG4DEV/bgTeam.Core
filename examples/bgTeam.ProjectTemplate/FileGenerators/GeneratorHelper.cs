namespace bgTeam.ProjectTemplate.FileGenerators
{
    using System;
    using System.IO;

    public class GeneratorHelper
    {
        public static readonly char Separator = Path.DirectorySeparatorChar;
        public static readonly string NewLine = Environment.NewLine;

        public static void CopyFile(string sourceFileName, string destFileName)
        {
            File.Copy(sourceFileName, destFileName);
            Console.WriteLine($"File copied {sourceFileName} -> {destFileName}");
        }

        public static void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
            Console.WriteLine($"File created {path}");
        }
    }
}
