using bgTeam.Core.Tests.Infrastructure;
using DapperExtensions;
using DapperExtensions.Mapper.Sql;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace bgTeam.Core.Tests.Tests.Dapper
{
    [Collection("SqlLiteCollection")]
    public class DapperHelperTests
    {
        [Fact]
        public void DefaultMapper()
        {
            DapperHelper.DefaultMapper = typeof(TestEntity);
            var mapperType = DapperHelper.DefaultMapper;
            Assert.Equal(typeof(TestEntity), mapperType);
        }

        [Fact]
        public void IdentityColumn()
        {
            DapperHelper.IdentityColumn = "Id";
            var column = DapperHelper.IdentityColumn;
            Assert.Equal("Id", column);
        }

        [Fact]
        public void Get()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            var entity = connection.Object.Get<TestEntity>(10);
            dapper.Verify(x => x.GetAsync<TestEntity>(It.IsAny<IDbConnection>(), 10, It.IsAny<IDbTransaction>(), null));
        }

        [Fact]
        public void GetAsync()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            var entity = connection.Object.GetAsync<TestEntity>(10);
            dapper.Verify(x => x.GetAsync<TestEntity>(It.IsAny<IDbConnection>(), 10, It.IsAny<IDbTransaction>(), null));
        }

        [Fact]
        public void GetAll()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            var entity = connection.Object.GetAll<TestEntity>();
            dapper.Verify(x => x.GetAllAsync<TestEntity>(It.IsAny<IDbConnection>(), It.IsAny<PredicateGroup>(), null, null, null, false));
        }

        [Fact]
        public void GetAllAsync()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            var entity = connection.Object.GetAllAsync<TestEntity>();
            dapper.Verify(x => x.GetAllAsync<TestEntity>(It.IsAny<IDbConnection>(), It.IsAny<PredicateGroup>(), null, null, null, false));
        }

        [Fact]
        public void Insert()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.Insert<TestEntity>(new[] { new TestEntity() });
            dapper.Verify(x => x.Insert(It.IsAny<IDbConnection>(), It.IsAny<IEnumerable<TestEntity>>(), null, null));
        }

        [Fact]
        public void Delete()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.Delete(new TestEntity());
            dapper.Verify(x => x.Delete(It.IsAny<IDbConnection>(), It.IsAny<TestEntity>(), null, null));
        }

        [Fact]
        public void DeleteByPredicate()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.Delete<TestEntity>(new FieldPredicate<TestEntity>());
            dapper.Verify(x => x.Delete(It.IsAny<IDbConnection>(), It.IsAny<object>(), null, null));
        }

        [Fact]
        public void GetSet()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.GetSet<TestEntity>(null, null, 10, 10);
            dapper.Verify(x => x.GetSet<TestEntity>(It.IsAny<IDbConnection>(), null, null, 10, 10, null, null, false));
        }

        [Fact]
        public void Count()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.Count<TestEntity>(null);
            dapper.Verify(x => x.Count<TestEntity>(It.IsAny<IDbConnection>(), null, null, null));
        }

        [Fact]
        public void GetMultiple()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            connection.Object.GetMultiple(null);
            dapper.Verify(x => x.GetMultiple(It.IsAny<IDbConnection>(), null, null, null));
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var dapper = new Mock<IDapperImplementor>();
            var connection = new Mock<IDbConnection>();
            DapperHelper.InstanceFactory = (conf) => dapper.Object;
            await connection.Object.UpdateAsync(new TestEntity(), x => x.Name == "Jack");
            dapper.Verify(x => x.UpdateAsync(It.IsAny<IDbConnection>(), It.IsAny<TestEntity>(), It.IsAny<PredicateGroup>(), null, null));
        }


        [Fact]
        public void GetMap()
        {
            var dapperExtensionsConfiguration = SetupDapperFactoryAndReturnItConfiguration();
            DapperHelper.GetMap<TestEntity>();
            dapperExtensionsConfiguration.Verify(x => x.GetMap<TestEntity>());
        }

        [Fact]
        public void ClearCache()
        {
            var dapperExtensionsConfiguration = SetupDapperFactoryAndReturnItConfiguration();
            DapperHelper.ClearCache();
            dapperExtensionsConfiguration.Verify(x => x.ClearCache());
        }

        [Fact]
        public void GetNextGuid()
        {
            var dapperExtensionsConfiguration = SetupDapperFactoryAndReturnItConfiguration();
            DapperHelper.GetNextGuid();
            dapperExtensionsConfiguration.Verify(x => x.GetNextGuid());
        }

        private Mock<IDapperExtensionsConfiguration> SetupDapperFactoryAndReturnItConfiguration()
        {
            var dapper = new Mock<IDapperImplementor>();
            var sqlGenerator = new Mock<ISqlGenerator>();
            var dapperExtensionsConfiguration = new Mock<IDapperExtensionsConfiguration>();

            sqlGenerator.SetupGet(x => x.Configuration)
                .Returns(dapperExtensionsConfiguration.Object);
            dapper.SetupGet(x => x.SqlGenerator)
                .Returns(sqlGenerator.Object);
            DapperHelper.InstanceFactory = (conf) => dapper.Object;

            return dapperExtensionsConfiguration;
        }
    }
}
