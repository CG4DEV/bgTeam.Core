using bgTeam.DataAccess.Impl.Sqlite;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl
{
    public class ConnectionFactorySqliteTests
    {
        [Fact]
        public void CreateUsingConnectionString()
        {
            var logger = new Mock<IAppLogger>();
            var factory = new ConnectionFactorySqlite(logger.Object, "Data Source=testSqlite.db;", null);
            using (var connection = factory.Create("Data Source=testSqlite2.db;"))
            {
                Assert.NotNull(connection);
                Assert.Equal("Data Source=testSqlite2.db;", connection.ConnectionString);
                Assert.Equal(ConnectionState.Open, connection.State);
            }
        }
    }
}
