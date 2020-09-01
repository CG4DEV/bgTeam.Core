namespace bgTeam.StoryRunnerScheduler
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using bgTeam.Core.Helpers;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Program<T>
        where T : Startup
    {
        protected IWebHost _host;
        protected Runner _runner;
        protected IAppLogger _logger;
        protected Dictionary<string, string> _args;

        public IServiceProvider ServiceProvider { get; private set; }

        public void Init(string[] args)
        {
            _args = CommandLineHelper.ParseArgs(args);

            var builder = CreateWebHostBuilder(args);

            _host = builder.Build();
            ServiceProvider = _host.Services;

            _logger = ServiceProvider.GetService<IAppLogger>();
            _runner = ServiceProvider.GetService<Runner>();

            var process = Process.GetCurrentProcess();
            process.EnableRaisingEvents = true;

            AppDomain.CurrentDomain.ProcessExit += OnExit;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        public void Run()
        {
            var hostTask = _host.RunAsync();

            _runner.Run();

            Task.WaitAll(hostTask);

            _logger.Error("Program is closed!");
        }

        protected virtual IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostingConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("hosting.json", optional: false)
                .AddCommandLine(args)
                .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(hostingConfig)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var tempBuilder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", optional: true)
                        .AddEnvironmentVariables()
                        .Build();

                    var path = tempBuilder["AppConfigsPath"];
                    var configs = tempBuilder["AppConfigsAdditional"];

                    if (!(string.IsNullOrEmpty(path) || string.IsNullOrEmpty(configs)))
                    {
                        foreach (var item in configs.Split(','))
                        {
                            builder.AddJsonFile(Path.Combine(path, $"{item.Trim()}.{context.HostingEnvironment.EnvironmentName}.json"), optional: true);
                        }
                    }
                })
                .UseStartup<T>();
        }

        protected virtual void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _logger.Error($"Unobserved task exception: {e.Exception}");
            e.SetObserved();
        }

        protected virtual void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            if (e.IsTerminating)
            {
                _logger.Fatal(ex);
            }
            else
            {
                _logger.Error(ex);
            }
        }

        protected virtual void OnExit(object sender, EventArgs e)
        {
            _runner?.Stop();
        }
    }
}