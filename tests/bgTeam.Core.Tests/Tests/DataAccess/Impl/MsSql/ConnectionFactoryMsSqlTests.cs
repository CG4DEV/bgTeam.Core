using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.MsSql;
using Moq;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.MsSql
{
    public class ConnectionFactoryOracleTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new ConnectionFactoryMsSql(null, connectionSetting.Object);
            });
        }

        [Fact]
        public void DependencyConnectionSetting()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryMsSql(appLogger.Object, (IConnectionSetting)null);
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
                new ConnectionFactoryMsSql(appLogger.Object, connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryMsSql(appLogger.Object, "Server=myServerName,myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword");
            Assert.Throws<SqlException>(() => connectionFactoryMsSql.Create());
        }

        [Fact]
        public async Task CreateAsync()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryMsSql(appLogger.Object, "Server=myServerName,myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword");
            await Assert.ThrowsAsync<SqlException>(() => connectionFactoryMsSql.CreateAsync());
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
