namespace bgTeam.DataAccess.Impl
{
    public class ConnectionSettingDefault : IConnectionSetting
    {
        public ConnectionSettingDefault(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
