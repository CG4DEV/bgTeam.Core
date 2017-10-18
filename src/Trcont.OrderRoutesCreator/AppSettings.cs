namespace Trcont.OrderRoutesCreator
{
    using bgTeam.Core;
    using bgTeam.DataAccess;

    public interface IApplicationSetting
    {

    }

    public class AppSettings : IApplicationSetting, IConnectionSetting
    {
        private readonly IAppConfiguration _appConfiguration;

        public string ConnectionString { get; set; }

        public AppSettings(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            ConnectionString = appConfiguration.GetConnectionString("RISDB");
        }
    }
}
