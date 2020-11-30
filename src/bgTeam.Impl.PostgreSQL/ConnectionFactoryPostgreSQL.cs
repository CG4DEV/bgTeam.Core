namespace bgTeam.DataAccess.Impl.PostgreSQL
{
    using System;
    using System.Data;
    using System.Threading.Tasks;
    using Npgsql;

    public class ConnectionFactoryPostgreSQL : IConnectionFactory
    {
        private readonly IConnectionSetting _setting;

        public ConnectionFactoryPostgreSQL(
            IConnectionSetting setting,
            ISqlDialect dialect = null)
        {
            _setting = setting ?? throw new ArgumentNullException(nameof(setting));
            if (string.IsNullOrEmpty(_setting.ConnectionString))
            {
                throw new ArgumentOutOfRangeException(nameof(setting), "Connection string should not be null or empty");
            }

            if (dialect != null)
            {
                dialect.Init(SqlDialectEnum.PostgreSql);
            }
        }

        public ConnectionFactoryPostgreSQL(
            string connectionString,
            ISqlDialect dialect = null)
            : this(new ConnectionSettingDefault(connectionString), dialect)
        {
        }

        public IDbConnection Create()
        {
            return Create(_setting.ConnectionString);
        }

        public IDbConnection Create(string connectionString)
        {
            NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        public async Task<IDbConnection> CreateAsync()
        {
            return await CreateAsync(_setting.ConnectionString);
        }

        public async Task<IDbConnection> CreateAsync(string connectionString)
        {
            NpgsqlConnection dbConnection = new NpgsqlConnection(connectionString);
            await dbConnection.OpenAsync().ConfigureAwait(false);
            return dbConnection;
        }
    }
}
