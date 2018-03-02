namespace bgTeam.DataAccess.Impl
{
    /// <summary>
    /// Реализация получения настроек подключения к БД по-умолчанию
    /// </summary>
    public class ConnectionSettingDefault : IConnectionSetting
    {
        public ConnectionSettingDefault(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }
    }
}
