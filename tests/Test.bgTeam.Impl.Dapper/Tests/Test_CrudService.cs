using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.Sqlite;
using bgTeam.Impl;
using System.Linq;
using System.Threading.Tasks;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;

namespace Test.bgTeam.Impl.Dapper.Tests
{
    public class Test_CrudService
    {
        private readonly FactoryTestService _factory;

        public Test_CrudService()
        {
            _factory = new FactoryTestService();
        }

        [Fact]
        public async Task Test_InsertAsync()
        {
            ICrudService serv = new CrudServiceDapper(_factory.ConnectionFactory);

            await serv.ExecuteAsync(@"CREATE TABLE 'TestEntity'
                                (
                                    [Id] INTEGER  NOT NULL PRIMARY KEY,
                                    [Name] TEXT  NULL
                                )");

            int res1 = await serv.InsertAsync(new TestEntity { Id = 1, Name = "First test entity" });
            int res2 = await serv.InsertAsync(new TestEntity { Id = 2, Name = "Second test entity" });
            int res3 = await serv.InsertAsync(new TestEntity { Id = 3, Name = "Third test entity" });

            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            int insertedRecordsCount = (await rep.GetAllAsync<TestEntity>(x => x.Id == 1 || x.Id == 2)).Count();
            TestEntity insertedEntity = await rep.GetAsync<TestEntity>(x => x.Id == 3);

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.Equal(3, res3);
            Assert.NotNull(insertedEntity);
            Assert.Equal(3, insertedEntity.Id);
            Assert.Equal("Third test entity", insertedEntity.Name);
            Assert.Equal(2, insertedRecordsCount);
        }

        [Fact]
        public async Task Test_UpdateAsync()
        {
            ICrudService serv = new CrudServiceDapper(_factory.ConnectionFactory);

            await serv.ExecuteAsync(@"CREATE TABLE 'TestEntity'
                                (
                                    [Id] INTEGER  NOT NULL PRIMARY KEY,
                                    [Name] TEXT  NULL
                                )");

            int res1 = await serv.InsertAsync(new TestEntity { Id = 1, Name = "First test entity" });
            int res2 = await serv.InsertAsync(new TestEntity { Id = 2, Name = "Second test entity" });
            int res3 = await serv.InsertAsync(new TestEntity { Id = 3, Name = "Third test entity" });

            bool res4 = await serv.UpdateAcync(new TestEntity { Id = 3, Name = "Third test entity UPDATED" });

            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            TestEntity updatedEntity = await rep.GetAsync<TestEntity>(x => x.Id == 3);

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.Equal(3, res3);
            Assert.True(res4);
            Assert.NotNull(updatedEntity);
            Assert.Equal(3, updatedEntity.Id);
            Assert.Equal("Third test entity UPDATED", updatedEntity.Name);
            Assert.NotEqual("Third test entity", updatedEntity.Name);
        }

        [Fact]
        public async Task Test_DeleteAsync()
        {
            ICrudService serv = new CrudServiceDapper(_factory.ConnectionFactory);

            await serv.ExecuteAsync(@"CREATE TABLE 'TestEntity'
                                (
                                    [Id] INTEGER  NOT NULL PRIMARY KEY,
                                    [Name] TEXT  NULL
                                )");

            int res1 = await serv.InsertAsync(new TestEntity { Id = 1, Name = "First test entity" });
            int res2 = await serv.InsertAsync(new TestEntity { Id = 2, Name = "Second test entity" });
            int res3 = await serv.InsertAsync(new TestEntity { Id = 3, Name = "Third test entity" });

            bool res4 = await serv.DeleteAcync(new TestEntity { Id = 3 });

            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            TestEntity deletedEntity = await rep.GetAsync<TestEntity>(x => x.Id == 3);

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.Equal(3, res3);
            Assert.True(res4);
            Assert.Null(deletedEntity);            
        }
    }
}
