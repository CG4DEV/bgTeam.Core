using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.PostgreSQL;
using Moq;
using Xunit;

namespace bgTeam.Core.Tests.Tests.DataAccess.Impl.PostgreSQL
{
    public class ConnectionFactoryPostgreSQLTests
    {
        [Fact]
        public void DependencyConnectionSetting()
        {
            var connectionSetting = GetMocks();
            Assert.Throws<ArgumentNullException>("setting", () =>
            {
                new ConnectionFactoryPostgreSQL((IConnectionSetting)null);
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
                new ConnectionFactoryPostgreSQL(connectionString);
            });
        }

        [Fact]
        public void Create()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryPostgreSQL("User ID=root;Password=myPassword;Host=somehost;Port=5432;Database=myDataBase;");
            Assert.Throws<SocketException>(() => connectionFactoryMsSql.Create());
        }

        [Fact]
        public async Task CreateAsync()
        {
            var connectionSetting = GetMocks();
            var connectionFactoryMsSql = new ConnectionFactoryPostgreSQL("User ID=root;Password=myPassword;Host=somehost;Port=5432;Database=myDataBase;");
            await Assert.ThrowsAsync<SocketException>(() => connectionFactoryMsSql.CreateAsync());
        }

        private Mock<IConnectionSetting> GetMocks()
        {
            var connectionSetting = new Mock<IConnectionSetting>();
            return connectionSetting;
        }
    }
}
