using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.Sqlite;

namespace bgTeam.Core.Tests.Infrastructure
{
    internal class FactoryTestService
    {
        public IConnectionFactory ConnectionFactory { get; private set; }

        public string ConnectionString { get; } = "Data Source=testSqlite.db;";

        public ISqlDialect Dialect { get; private set; }

        public FactoryTestService()
        {
            Dialect = new SqlDialectDapper();
            Dialect.Init(SqlDialectEnum.Sqlite);

            ConnectionFactory = new ConnectionFactorySqlite(ConnectionString, Dialect);
        }
    }
}
