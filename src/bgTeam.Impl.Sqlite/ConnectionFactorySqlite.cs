namespace bgTeam.DataAccess.Impl.Sqlite
{
    using System.Data;
    using System.Data.SQLite;
    using System.Threading.Tasks;

    public class ConnectionFactorySqlite : IConnectionFactory
    {
        private readonly IConnectionSetting _setting;

        public ConnectionFactorySqlite(
            IConnectionSetting setting,
            ISqlDialect dialect = null)
        {
            _setting = setting;

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.Sqlite);
            }
        }

        public ConnectionFactorySqlite(
            string connectionString,
            ISqlDialect dialect = null)
            : this(new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return CreateAsync().Result;
        }

        public IDbConnection Create(string connectionString)
        {
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            return await CreateAsync(_setting.ConnectionString);
        }

        public async Task<IDbConnection> CreateAsync(string connectionString)
        {
            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);
            return dbConnection;
        }
    }
}
