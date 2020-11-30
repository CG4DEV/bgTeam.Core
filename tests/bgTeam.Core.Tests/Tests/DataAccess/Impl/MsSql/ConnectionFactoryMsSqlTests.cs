using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.MsSql;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.MsSql
{
    public class ConnectionFactoryOracleTests
    {
        [Fact]
        public void DependencyConnectionSetting()
        {
            var connectionSetting = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryMsSql((IConnectionSetting)null);
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
                new ConnectionFactoryMsSql(connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryMsSql("Server=myServerName,myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword");
            Assert.Throws<SqlException>(() => connectionFactoryMsSql.Create());
        }

        [Fact]
        public async Task CreateAsync()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryMsSql("Server=myServerName,myPortNumber;Database=myDataBase;UID=myUsername;PWD=myPassword");
            await Assert.ThrowsAsync<SqlException>(() => connectionFactoryMsSql.CreateAsync());
        }

        private Mock<IConnectionSetting> GetMocks()
        {
            var connectionSetting = new Mock<IConnectionSetting>();
            return (connectionSetting);
        }
    }
}
