namespace bgTeam.DataAccess.Impl.MsSql
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    public class ConnectionFactoryMsSql : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryMsSql(
            IAppLogger logger,
            IConnectionSetting setting,
            ISqlDialect dialect)
        {
            _logger = logger;
            _setting = setting;

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.MsSql);
            }
        }

        public ConnectionFactoryMsSql(
            IAppLogger logger,
            string connectionString,
            ISqlDialect dialect)
            : this(logger, new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return Create(_setting.ConnectionString);
        }

        public IDbConnection Create(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryMsSql: {connectionString}");

            SqlConnection dbConnection = new SqlConnection(connectionString);
            dbConnection.Open();

            _logger.Debug($"ConnectionFactoryMsSql: connect open");

            return dbConnection;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            return await CreateAsync(_setting.ConnectionString);
        }

        public async Task<IDbConnection> CreateAsync(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryMsSql: {connectionString}");

            SqlConnection dbConnection = new SqlConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);

            _logger.Debug($"ConnectionFactoryMsSql: connect open");

            return dbConnection;
        }

        ////DbConnection dbConnection = MiniProfiler.Current == null
        ////                                ? (DbConnection)sqlConnection
        ////                                : new ProfiledDbConnection(sqlConnection, MiniProfiler.Current);
    }
}
