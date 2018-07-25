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
    public class Test_Repository
    {
        private readonly FactoryTestService _factory;

        public Test_Repository()
        {
            _factory = new FactoryTestService();
        }

        [Fact]
        public async Task Test_GetAsync()
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
            
            TestEntity insertedEntity = await rep.GetAsync<TestEntity>(x => x.Id == 2);
            TestEntity notFoundEntity = await rep.GetAsync<TestEntity>(x => x.Id == 100);

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.Equal(3, res3);
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Equal("Second test entity", insertedEntity.Name);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        public async Task Test_GetAllAsync()
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

            var allEntities = await rep.GetAllAsync<TestEntity>();
            var emptyCollection = await rep.GetAllAsync<TestEntity>(x => x.Name == "qwerty");

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(1, res1);
            Assert.Equal(2, res2);
            Assert.Equal(3, res3);
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }
    }
}
