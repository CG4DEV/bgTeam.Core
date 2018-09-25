namespace bgTeam.ProjectTemplate
{
    using System.Threading.Tasks;
    using bgTeam.Core.Helpers;
    using Microsoft.Extensions.DependencyInjection;

    internal static class Program
    {
        internal static async Task Main(string[] args)
        {
            var cmdParams = CommandLineHelper.ParseArgs(args);

            var container = AppIocConfigure.Configure(cmdParams);
            var runner = container.GetService<Runner>();

            await runner.Run();
        }
    }
}
