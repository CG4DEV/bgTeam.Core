namespace Trcont.Cud.Story.User
{
    using bgTeam.DataAccess;
    using System.Configuration;

    internal class UserDBConnection : IConnectionSetting
    {
        private static bgTeam.Core.IAppConfiguration _conf = new bgTeam.Core.Impl.AppConfigurationDefault();

        public UserDBConnection()
        {
            ConnectionString = _conf.GetConnectionString("CUDDBSYS");
        }

        public string ConnectionString { get; set; }
    }
}
