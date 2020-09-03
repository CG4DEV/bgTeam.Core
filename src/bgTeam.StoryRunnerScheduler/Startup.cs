namespace bgTeam.StoryRunnerScheduler
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using Serilog.Events;

    public abstract class Startup
    {
        public IWebHostEnvironment HostingEnvironment { get; }

        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment env, IConfiguration config)
        {
            HostingEnvironment = env;
            Configuration = config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom
                .Configuration(Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .CreateLogger();

            services.AddLogging(b => b.AddSerilog(Log.Logger));

            services.AddSwaggerDocument(x =>
            {
                x.Title = "bgTeam.StoryRunnerScheduler";
                x.Description = "StoryRunnerScheduler";
                x.IgnoreObsoleteProperties = true;
                x.GenerateXmlObjects = true;
            });

            ConfigureServiceCollection(services);

            services.AddMvc(x => x.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseMvc();
        }

        public abstract void ConfigureServiceCollection(IServiceCollection services);
    }
}
