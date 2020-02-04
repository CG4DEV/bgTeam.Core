using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.MsSql;
using bgTeam.DataAccess.Impl.PostgreSQL;
using Moq;
using System;
using System.Data.SqlClient;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.PostgreSQL
{
    public class ConnectionFactoryPostgreSQLTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new ConnectionFactoryPostgreSQL(null, connectionSetting.Object);
            });
        }

        [Fact]
        public void DependencyConnectionSetting()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryPostgreSQL(appLogger.Object, (IConnectionSetting)null);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DependencyConnectionSettingConnectionString(string connectionString)
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentOutOfRangeException>("setting", () =>
            {
                new ConnectionFactoryPostgreSQL(appLogger.Object, connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryPostgreSQL(appLogger.Object, "User ID=root;Password=myPassword;Host=somehost;Port=5432;Database=myDataBase;");
            Assert.Throws<SocketException>(() => connectionFactoryMsSql.Create());
        }

        [Fact]
        public async Task CreateAsync()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryPostgreSQL(appLogger.Object, "User ID=root;Password=myPassword;Host=somehost;Port=5432;Database=myDataBase;");
            await Assert.ThrowsAsync<SocketException>(() => connectionFactoryMsSql.CreateAsync());
        }

        private (
            Mock<IAppLogger>,
            Mock<IConnectionSetting>)
            GetMocks()
        {
            var appLogger = new Mock<IAppLogger>();
            var connectionSetting = new Mock<IConnectionSetting>();
            return (
                appLogger,
                connectionSetting);
        }
    }
}
