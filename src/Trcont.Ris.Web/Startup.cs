namespace Trcont.Ris.Web
{
    using System;
    using bgTeam;
    using bgTeam.Core;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json.Serialization;
    using Trcont.Ris.Web.ErrorHandlers;
    using Trcont.Ris.WebApp;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => {
                    options.ModelBinderProviders.Insert(0, new InvariantDecimalModelBinderProvider());                              
                })
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                });
            services.AddRouting();
            services.Configure<IISOptions>(option =>
            {
                option.AutomaticAuthentication = false;
                option.AuthenticationDisplayName = "iSalesUser";
                option.ForwardClientCertificate = false;
            });

            DIContainer.Configure(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IAppConfiguration appConfiguration, IAppLogger appLogger)
        {
            var envName = appConfiguration["BuildConfigureVariable"] ?? string.Empty;

            if (env.IsDevelopment() || Environment.GetEnvironmentVariable(envName).Equals("Debug"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            //app.UseExceptionHandler(new ExceptionHandlerOptions() { ExceptionHandler = (c) => { c.} });

            app.UseMvc(x =>
            {
                x.MapRoute(name: "default",
                           template: "{controller=Home}/{action=Index}");
            });
        }
    }
}
