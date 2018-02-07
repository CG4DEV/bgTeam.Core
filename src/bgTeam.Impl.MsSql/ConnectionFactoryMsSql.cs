namespace bgTeam.Infrastructure.DataAccess
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using bgTeam;
    using bgTeam.DataAccess;
    using bgTeam.DataAccess.Impl;

    public class ConnectionFactoryMsSql : IConnectionFactory
    {
        private readonly IAppLogger _logger;
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryMsSql(
            IAppLogger logger,
            IConnectionSetting setting)
        {
            _logger = logger;
            _setting = setting;

            // TODO : Возможно стоит разместить в другом месте
            //DapperHelper.IdentityColumn = "Id";
            //DapperHelper.SqlDialect = new SqlServerDialect();
        }

        public ConnectionFactoryMsSql(
            IAppLogger logger,
            string connectionString)
            : this(logger, new ConnectionSettingDefault(connectionString))
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
