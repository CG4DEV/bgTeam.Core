namespace bgTeam.DataAccess.Impl
{
    public class ConnectionSettingDefault : IConnectionSetting
    {
        public string ConnectionString { get; set; }

        public ConnectionSettingDefault(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
