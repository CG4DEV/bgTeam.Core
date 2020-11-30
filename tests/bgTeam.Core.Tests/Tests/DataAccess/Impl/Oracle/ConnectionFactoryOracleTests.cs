using System;
using System.Threading.Tasks;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Oracle;
using Moq;
using Oracle.ManagedDataAccess.Client;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.Oracle
{
    public class ConnectionFactoryOracleTests
    {
        [Fact]
        public void DependencyConnectionSetting()
        {
            var connectionSetting = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryOracle((IConnectionSetting)null);
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void DependencyConnectionSettingConnectionString(string connectionString)
        {
            var connectionSetting = GetMocks();
            Assert.Throws<ArgumentOutOfRangeException>("setting", () =>
            {
                new ConnectionFactoryOracle(connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryOracle("Data Source=MyOracleDB;");
            var ex = Assert.Throws<AggregateException>(() => connectionFactoryMsSql.Create()).InnerException;
            Assert.Equal(12154, ((OracleException)ex).Number);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryOracle("Data Source=MyOracleDB;");
            var ex = await Assert.ThrowsAsync<OracleException>(() => connectionFactoryMsSql.CreateAsync());
            Assert.Equal(12154, ex.Number);
        }

        private Mock<IConnectionSetting> GetMocks()
        {
            var connectionSetting = new Mock<IConnectionSetting>();
            return connectionSetting;
        }
    }
}
