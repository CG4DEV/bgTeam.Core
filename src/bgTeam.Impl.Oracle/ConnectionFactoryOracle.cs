namespace bgTeam.DataAccess.Impl.Oracle
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using global::Oracle.ManagedDataAccess.Client;

    public class ConnectionFactoryOracle : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryOracle(
            IAppLogger logger,
            IConnectionSetting setting,
            ISqlDialect dialect = null)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
            if (string.IsNullOrEmpty(_setting.ConnectionString))
            {
                throw new ArgumentOutOfRangeException(nameof(setting), "Connection string should not be null or empty");
            }

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.Oracle);
            }
        }

        public ConnectionFactoryOracle(
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
