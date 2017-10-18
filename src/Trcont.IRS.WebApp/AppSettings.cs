namespace Trcont.IRS.WebApp
{
    using bgTeam.DataAccess;
    using bgTeam.Core;

    internal class AppSettings : IConnectionSetting
    {
        private readonly IAppConfiguration _appConfiguration;

        public string ConnectionString { get; set; }

        public AppSettings(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            ConnectionString = appConfiguration.GetConnectionString("IRSDB");
        }
    }
}