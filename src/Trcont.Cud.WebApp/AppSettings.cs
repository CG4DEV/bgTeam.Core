namespace Trcont.Cud.WebApp
{
    using System.Configuration;
    using bgTeam.DataAccess;
    using bgTeam.Core;

    internal class AppSettings : IConnectionSetting, IAppSettings
    {
        private readonly IAppConfiguration _appConfiguration;

        public string ConnectionString { get; set; }

        public string ServerApiPart { get; set; }

        public AppSettings(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            ConnectionString = appConfiguration.GetConnectionString("CUDDB");
            ServerApiPart = appConfiguration["serverApiPart"];
        }
    }
}