namespace bgTeam.DataAccess.Impl.PostgreSQL
{
    using System.Data;
    using System.Threading.Tasks;
    using Npgsql;

    public class ConnectionFactoryPostgreSQL : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryPostgreSQL(
            IAppLogger logger,
            IConnectionSetting setting,
            ISqlDialect dialect = null)
        {
            _logger = logger;
            _setting = setting;

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.PostgreSql);
            }
        }

        public ConnectionFactoryPostgreSQL(
            IAppLogger logger,
            string connectionString,
            ISqlDialect dialect = null)
            : this(logger, new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return Create(_setting.ConnectionString);
        }

        public IDbConnection Create(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryPostgreSQL: {connectionString}");

            NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);
            dbConnection.Open();

            _logger.Debug($"ConnectionFactoryPostgreSQL: connect open");

            return dbConnection;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            return await CreateAsync(_setting.ConnectionString);
        }

        public async Task<IDbConnection> CreateAsync(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryPostgreSQL: {connectionString}");

            NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);

            _logger.Debug($"ConnectionFactoryPostgreSQL: connect open");

            return dbConnection;
        }
    }
}
