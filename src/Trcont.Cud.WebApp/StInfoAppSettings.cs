namespace Trcont.Cud.WebApp
{
    using bgTeam.Core;
    using Trcont.Cud.Infrastructure;

    public class StInfoAppSettings : IStInfoConnectionSetting
    {
        private readonly IAppConfiguration _appConfiguration;

        public string ConnectionString { get; set; }

        public StInfoAppSettings(IAppConfiguration appConfiguration)
        {
            ConnectionString = appConfiguration.GetConnectionString("CUDDBSTINFO");
        }
    }
}
