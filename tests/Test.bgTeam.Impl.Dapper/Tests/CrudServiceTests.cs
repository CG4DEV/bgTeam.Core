using bgTeam.DataAccess.Impl;
using System.Linq;
using System.Threading.Tasks;
using Test.bgTeam.Impl.Dapper.Common;
using Test.bgTeam.Impl.Dapper.Common.Attributes;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests
{
    [Collection("SqlLiteCollection")]
    public class CrudServiceTests
    {
        private static SqlLiteFixture _fixture;

        public CrudServiceTests(SqlLiteFixture crudServiceFixture)
        {
            _fixture = crudServiceFixture;
        }

        [Fact]
        [WithRecreatingTable]
        public async Task InsertAsync()
        {
            int res1 = await _fixture.CrudService.InsertAsync(new TestEntity { Name = "First test entity" });
            int res2 = await _fixture.CrudService.InsertAsync(new TestEntity { Name = "Second test entity" });
            using (var connection = _fixture.Factory.ConnectionFactory.Create()) 
            {
                int res3 = await _fixture.CrudService.InsertAsync(new TestEntity { Name = "Third test entity" }, connection);
                Assert.Equal(3, res3);
            }

            int insertedRecordsCount = (await _fixture.Repository.GetAllAsync<TestEntity>(x => x.Id == 1 || x.Id == 2)).Count();
            TestEntity insertedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.NotNull(insertedEntity);
            Assert.Equal(3, insertedEntity.Id);
            Assert.Equal("Third test entity", insertedEntity.Name);
            Assert.Equal(2, insertedRecordsCount);
        }

        [Fact]
        [WithRecreatingTable]
        public async Task Insert()
        {
            _fixture.CrudService.Insert(new TestEntity { Name = "First test entity" });
            _fixture.CrudService.Insert(new TestEntity { Name = "Second test entity" });
            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                _fixture.CrudService.Insert(new TestEntity { Name = "Third test entity" }, connection);
            }

            int insertedRecordsCount = (await _fixture.Repository.GetAllAsync<TestEntity>(x => x.Id == 1 || x.Id == 2)).Count();
            TestEntity insertedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);

            Assert.NotNull(insertedEntity);
            Assert.Equal(3, insertedEntity.Id);
            Assert.Equal("Third test entity", insertedEntity.Name);
            Assert.Equal(2, insertedRecordsCount);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task UpdateAsync()
        {
            bool res4 = await _fixture.CrudService.UpdateAsync(new TestEntity { Id = 3, Name = "1" });
            TestEntity updatedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);
            Assert.True(res4);
            Assert.NotNull(updatedEntity);
            Assert.Equal(3, updatedEntity.Id);
            Assert.Equal("1", updatedEntity.Name);

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                await _fixture.CrudService.UpdateAsync(new TestEntity { Id = 3, Name = "2" }, connection);
                (await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3, connection)).Name = "2";
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task Update()
        {
            bool res4 = _fixture.CrudService.Update(new TestEntity { Id = 3, Name = "3" });
            TestEntity updatedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);
            Assert.True(res4);
            Assert.NotNull(updatedEntity);
            Assert.Equal(3, updatedEntity.Id);
            Assert.Equal("3", updatedEntity.Name);

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                _fixture.CrudService.Update(new TestEntity { Id = 3, Name = "2" }, connection);
                (await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3, connection)).Name = "2";
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task DeleteAsync()
        {
            bool res4 = await _fixture.CrudService.DeleteAsync(new TestEntity { Id = 3 });
            TestEntity deletedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);
            Assert.True(res4);
            Assert.Null(deletedEntity);

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                await _fixture.CrudService.DeleteAsync(new TestEntity { Id = 2 }, connection);
                Assert.Null(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }

        [Fact]
        [WithRecreatingTable]
        public async Task ExecuteWithSqlObject()
        {
            Assert.Equal(1, _fixture.CrudService.Execute(new SqlObjectDefault(@"insert into TestEntity('name') values ('1')")));
            Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 1));

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                Assert.Equal(1, _fixture.CrudService.Execute(new SqlObjectDefault(@"insert into TestEntity('name') values ('1')"), connection));
                Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }

        [Fact]
        [WithRecreatingTable]
        public async Task ExecuteWithSqlString()
        {
            Assert.Equal(1, _fixture.CrudService.Execute(@"insert into TestEntity('name') values ('1')"));
            Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 1));

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                Assert.Equal(1, _fixture.CrudService.Execute(@"insert into TestEntity('name') values ('1')", null, connection));
                Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }

        [Fact]
        [WithRecreatingTable]
        public async Task ExecuteAsyncWithStringSql()
        {
            Assert.Equal(1, await _fixture.CrudService.ExecuteAsync(@"insert into TestEntity('name') values ('1')"));
            Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 1));

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                Assert.Equal(1, await _fixture.CrudService.ExecuteAsync(@"insert into TestEntity('name') values ('1')", null, connection));
                Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }

        [Fact]
        [WithRecreatingTable]
        public async Task ExecuteAsyncWithObjectSql()
        {
            Assert.Equal(1, await _fixture.CrudService.ExecuteAsync(new SqlObjectDefault(@"insert into TestEntity('name') values ('1')")));
            Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 1));

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                Assert.Equal(1, await _fixture.CrudService.ExecuteAsync(new SqlObjectDefault(@"insert into TestEntity('name') values ('1')"), connection));
                Assert.NotNull(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task Delete()
        {
            bool res4 = _fixture.CrudService.Delete(new TestEntity { Id = 3 });
            TestEntity deletedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 3);
            Assert.True(res4);
            Assert.Null(deletedEntity);

            using (var connection = _fixture.Factory.ConnectionFactory.Create())
            {
                _fixture.CrudService.Delete(new TestEntity { Id = 2 }, connection);
                Assert.Null(await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2, connection));
            }
        }
    }
}
