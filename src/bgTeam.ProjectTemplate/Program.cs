namespace bgTeam.ProjectTemplate
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using bgTeam.Core.Helpers;
    using Microsoft.Extensions.DependencyInjection;

    class Program
    {
        static void Main(string[] args)
        {
            var cmdParams = CommandLineHelper.ParseArgs(args);
            var process = Process.GetCurrentProcess();

            var container = AppIocConfigure.Configure(cmdParams);
            var runner = container.GetService<Runner>();

            runner.Run().Wait();
        }
    }
}
