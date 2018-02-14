namespace bgTeam.Impl.Oracle
{
    using System.Data;
    using System.Threading.Tasks;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;
    using global::Oracle.ManagedDataAccess.Client;

    public class ConnectionFactoryOracle : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryOracle(
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

        public ConnectionFactoryOracle(
            IAppLogger logger,
            string connectionString,
            ISqlDialect dialect)
            : this(logger, new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return CreateAsync().Result;
        }

        public IDbConnection Create(string connectionString)
        {
            _logger.Debug($"ConnectionFactoryMsSql: {connectionString}");

            OracleConnection dbConnection = new OracleConnection(connectionString);
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

            OracleConnection dbConnection = new OracleConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);

            _logger.Debug($"ConnectionFactoryMsSql: connect open");

            return dbConnection;
        }
    }
}
