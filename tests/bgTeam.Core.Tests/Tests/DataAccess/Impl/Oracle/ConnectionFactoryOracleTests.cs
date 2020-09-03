using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Oracle;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.Oracle
{
    public class ConnectionFactoryOracleTests
    {
        [Fact]
        public void DependencyAppLogger()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("logger", () =>
            {
                new ConnectionFactoryOracle(null, connectionSetting.Object);
            });
        }

        [Fact]
        public void DependencyConnectionSetting()
        {
            var (appLogger, connectionSetting) = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryOracle(appLogger.Object, (IConnectionSetting)null);
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
                new ConnectionFactoryOracle(appLogger.Object, connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryOracle(appLogger.Object, "Data Source=MyOracleDB;");
            var ex = Assert.Throws<AggregateException>(() => connectionFactoryMsSql.Create()).InnerException;
            Assert.Equal(12154, ((OracleException)ex).Number);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var (appLogger, connectionSetting) = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryOracle(appLogger.Object, "Data Source=MyOracleDB;");
            var ex = await Assert.ThrowsAsync<OracleException>(() => connectionFactoryMsSql.CreateAsync());
            Assert.Equal(12154, ex.Number);
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
