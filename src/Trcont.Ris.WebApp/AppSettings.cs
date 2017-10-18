namespace Trcont.Ris.WebApp
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.Core;

    internal interface IAppSettings
    {

    }

    internal class AppSettings : IConnectionSetting, IAppSettings
    {
        private readonly IAppConfiguration _appConfiguration;

        public AppSettings(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;
            ConnectionString = _appConfiguration.GetConnectionString("RISDB");
        }

        public string ConnectionString { get; set; }

        public AppSettings()
        {
            ConnectionString = _appConfiguration.GetConnectionString("RISDB");
        }
    }
}
