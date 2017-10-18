namespace Trcont.Cud.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Trcont.Cud.WebApp;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(x =>
                x.MapRoute(name: "default", template: "{controller=Home}/{action=Index}"));
        }
    }
}
