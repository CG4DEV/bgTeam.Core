namespace bgTeam.DataAccess.Impl.Sqlite
{
    using System.Data;
    using System.Data.SQLite;
    using System.Threading.Tasks;

    public class ConnectionFactorySqlite : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactorySqlite(
            IAppLogger logger,
            IConnectionSetting setting,
            ISqlDialect dialect = null)
        {
            _logger = logger;
            _setting = setting;

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.Sqlite);
            }
        }

        public ConnectionFactorySqlite(
            IAppLogger logger,
            string connectionString,
            ISqlDialect dialect = null)
            : this(logger, new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return CreateAsync().Result;
        }

        public IDbConnection Create(string connectionString)
        {
            _logger.Debug($"ConnectionFactorySqlLite: {connectionString}");

            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();

            _logger.Debug($"ConnectionFactorySqlLite: connect open");

            return dbConnection;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            return await CreateAsync(_setting.ConnectionString);
        }

        public async Task<IDbConnection> CreateAsync(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryMsSql: {connectionString}");

            SQLiteConnection dbConnection = new SQLiteConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);

            _logger.Debug($"ConnectionFactoryMsSql: connect open");

            return dbConnection;
        }
    }
}
