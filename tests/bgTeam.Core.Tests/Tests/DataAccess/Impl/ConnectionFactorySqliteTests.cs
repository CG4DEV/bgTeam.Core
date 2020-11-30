using System.Data;
using bgTeam.DataAccess.Impl.Sqlite;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl
{
    public class ConnectionFactorySqliteTests
    {
        [Fact]
        public void CreateUsingConnectionString()
        {
            var factory = new ConnectionFactorySqlite("Data Source=testSqlite.db;", null);
            using (var connection = factory.Create("Data Source=testSqlite2.db;"))
            {
                Assert.NotNull(connection);
                Assert.Equal("Data Source=testSqlite2.db;", connection.ConnectionString);
                Assert.Equal(ConnectionState.Open, connection.State);
            }
        }
    }
}
