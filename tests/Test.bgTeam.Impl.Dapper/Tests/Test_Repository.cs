using System.Collections.Generic;
using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using bgTeam.DataAccess.Impl.Sqlite;
using bgTeam.Impl;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
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

        [Fact]
        public async Task Test_GetPageAsync()
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
            int res4 = await serv.InsertAsync(new TestEntity { Id = 4, Name = "Fourth test entity" });
            int res5 = await serv.InsertAsync(new TestEntity { Id = 5, Name = "Fifth test entity" });
            int res6 = await serv.InsertAsync(new TestEntity { Id = 6, Name = "Sixth test entity" });

            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            var sort = new List<ISort>()
                {  Predicates.Sort<TestEntity>(x => x.Id) };

            var firstPage = (await rep.GetPageAsync<TestEntity>(null, sort, 0, 2)).ToList();
            var secondPage = (await rep.GetPageAsync<TestEntity>(null, sort, 1, 2)).ToList();
            var thirdPage = (await rep.GetPageAsync<TestEntity>(null, sort, 2, 2)).ToList();

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Equal(2, firstPage.Count());
            Assert.Equal(1, firstPage.First().Id);
            Assert.Equal(2, firstPage.Skip(1).First().Id);

            Assert.Equal(2, secondPage.Count());
            Assert.Equal(3, secondPage.First().Id);
            Assert.Equal(4, secondPage.Skip(1).First().Id);

            Assert.Equal(2, thirdPage.Count());
            Assert.Equal(5, thirdPage.First().Id);
            Assert.Equal(6, thirdPage.Skip(1).First().Id);
        }

        [Fact]
        public async Task Test_GetPageAsyncPredicates()
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
            int res4 = await serv.InsertAsync(new TestEntity { Id = 4, Name = "Fourth test entity" });
            int res5 = await serv.InsertAsync(new TestEntity { Id = 5, Name = "Third test entity" });
            int res6 = await serv.InsertAsync(new TestEntity { Id = 6, Name = "Sixth test entity" });


            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            var sort = new List<ISort>()
                {  Predicates.Sort<TestEntity>(x => x.Id, false) };

            var second = (await rep.GetPageAsync<TestEntity>(x => x.Id == 2, sort, 0, 10)).Single();
            var fifth = (await rep.GetPageAsync<TestEntity>(x => x.Id == 5 && x.Name == "Third test entity", sort, 0, 10)).Single();
            var thirdAndFifth = (await rep.GetPageAsync<TestEntity>(x => x.Name == "Third test entity", sort, 0, 10)).ToList();

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");


            Assert.Equal(2, second.Id);
            Assert.Equal(5, fifth.Id);
            Assert.Equal(2, thirdAndFifth.Count);
            Assert.Equal(5, thirdAndFifth.First().Id);
            Assert.Equal(3, thirdAndFifth.Skip(1).First().Id);
        }


        [Fact]
        public async Task Test_GeneratePagingSql()
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
            int res4 = await serv.InsertAsync(new TestEntity { Id = 4, Name = "Fourth test entity" });
            int res5 = await serv.InsertAsync(new TestEntity { Id = 5, Name = "Fifth test entity" });
            int res6 = await serv.InsertAsync(new TestEntity { Id = 6, Name = "Sixth test entity" });

            IRepository rep = new RepositoryDapper(_factory.ConnectionFactory);

            var sql = "SELECT * FROM TestEntity WHERE Id > @Value";

            var sqlObj = _factory.Dialect.GeneratePagingSql(sql, 1, 1, new { Value = 3 });

            var res = (await rep.GetAllAsync<TestEntity>(sqlObj)).ToList();

            await serv.ExecuteAsync(@"DROP TABLE 'TestEntity'");

            Assert.Single(res);
            Assert.Equal(5, res.Single().Id);
        }

    }
}
