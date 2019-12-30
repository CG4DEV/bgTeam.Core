using System.Collections.Generic;
using bgTeam.DataAccess;
using System.Linq;
using System.Threading.Tasks;
using DapperExtensions;
using Test.bgTeam.Impl.Dapper.Common;
using Xunit;
using Test.bgTeam.Impl.Dapper.Common.Attributes;
using bgTeam.DataAccess.Impl;

namespace Test.bgTeam.Impl.Dapper.Tests
{
    [Collection("SqlLiteCollection")]
    public class RepositoryTests
    {
        private static SqlLiteFixture _fixture;

        public RepositoryTests(SqlLiteFixture crudServiceFixture)
        {
            _fixture = crudServiceFixture;
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetBySqlObject()
        {
            TestEntity insertedEntity = _fixture.Repository.Get<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 2"));
            TestEntity notFoundEntity = _fixture.Repository.Get<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 100"));
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAsyncBySqlObject()
        {
            TestEntity insertedEntity = await _fixture.Repository.GetAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 2"));
            TestEntity notFoundEntity = await _fixture.Repository.GetAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 100"));
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetBySqlString()
        {
            TestEntity insertedEntity = _fixture.Repository.Get<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 2 });
            TestEntity notFoundEntity = _fixture.Repository.Get<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 100 });
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAsyncBySqlString()
        {
            TestEntity insertedEntity = await _fixture.Repository.GetAsync<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 2 });
            TestEntity notFoundEntity = await _fixture.Repository.GetAsync<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 100 });
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetAllBySqlObject()
        {
            var allEntities = _fixture.Repository.GetAll<TestEntity>(new SqlObjectDefault(@"select * from TestEntity"));
            var emptyCollection = _fixture.Repository.GetAll<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 123"));
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllBySqlObjectWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = _fixture.Repository.GetAll<TestEntity>(new SqlObjectDefault(@"select * from TestEntity"), connection);
                var emptyCollection = _fixture.Repository.GetAll<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 123"), connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncBySqlObject()
        {
            var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity"));
            var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 123"));
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncBySqlObjectWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity"), connection);
                var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(new SqlObjectDefault(@"select * from TestEntity where id = 123"), connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetAllBySqlString()
        {
            var allEntities = _fixture.Repository.GetAll<TestEntity>(@"select * from TestEntity");
            var emptyCollection = _fixture.Repository.GetAll<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 123 });
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllBySqlStringWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = _fixture.Repository.GetAll<TestEntity>(@"select * from TestEntity", null, connection);
                var emptyCollection = _fixture.Repository.GetAll<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 123 }, connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncBySqlString()
        {
            var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>(@"select * from TestEntity");
            var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 123 });
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncBySqlStringWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>(@"select * from TestEntity", null, connection);
                var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(@"select * from TestEntity where id = @Id", new { Id = 123 }, connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetByPredicate()
        {
            var entity = _fixture.Repository.Get<TestEntity>(x => x.Name == "1");
            var nonExistingEntity = _fixture.Repository.Get<TestEntity>(x => x.Name == "qwerty");
            Assert.NotNull(entity);
            Assert.Null(nonExistingEntity);
            Assert.Equal(1, entity.Id);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetByPredicateWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var entity = _fixture.Repository.Get<TestEntity>(x => x.Name == "1", connection);
                var nonExistingEntity = _fixture.Repository.Get<TestEntity>(x => x.Name == "qwerty", connection);
                Assert.NotNull(entity);
                Assert.Null(nonExistingEntity);
                Assert.Equal(1, entity.Id);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncByPredicate()
        {
            var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>();
            var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(x => x.Name == "qwerty");
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllAsyncByPredicateWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = await _fixture.Repository.GetAllAsync<TestEntity>(x => x.Id > 0, connection);
                var emptyCollection = await _fixture.Repository.GetAllAsync<TestEntity>(x => x.Name == "qwerty", connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities(6)]
        public void GetPage()
        {
            var sort = new List<ISort>()
            {
                Predicates.Sort<TestEntity>(x => x.Id)
            };

            var firstPage = _fixture.Repository.GetPage<TestEntity>(null, sort, 0, 2).ToList();
            var secondPage = _fixture.Repository.GetPage<TestEntity>(null, sort, 1, 2).ToList();
            var thirdPage = _fixture.Repository.GetPage<TestEntity>(null, sort, 2, 2).ToList();
            
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
        [WithRecreatingTable]
        [InsertTestEntities(6)]
        public async Task GetPageAsync()
        {
            var sort = new List<ISort>()
            {
                Predicates.Sort<TestEntity>(x => x.Id)
            };

            var firstPage = (await _fixture.Repository.GetPageAsync<TestEntity>(null, sort, 0, 2)).ToList();
            var secondPage = (await _fixture.Repository.GetPageAsync<TestEntity>(null, sort, 1, 2)).ToList();
            var thirdPage = (await _fixture.Repository.GetPageAsync<TestEntity>(null, sort, 2, 2)).ToList();

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
        [WithRecreatingTable]
        [InsertTestEntities]
        public void GetAllByPredicate()
        {
            var allEntities = _fixture.Repository.GetAll<TestEntity>();
            var emptyCollection = _fixture.Repository.GetAll<TestEntity>(x => x.Name == "qwerty");
            Assert.NotNull(allEntities);
            Assert.NotEmpty(allEntities);
            Assert.Equal(3, allEntities.Count());
            Assert.NotNull(emptyCollection);
            Assert.Empty(emptyCollection);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAllByPredicateWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var allEntities = _fixture.Repository.GetAll<TestEntity>(x => x.Id > 0, connection);
                var emptyCollection = _fixture.Repository.GetAll<TestEntity>(x => x.Name == "qwerty", connection);
                Assert.NotNull(allEntities);
                Assert.NotEmpty(allEntities);
                Assert.Equal(3, allEntities.Count());
                Assert.NotNull(emptyCollection);
                Assert.Empty(emptyCollection);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAsyncByPredicate()
        {
            TestEntity insertedEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 2);
            TestEntity notFoundEntity = await _fixture.Repository.GetAsync<TestEntity>(x => x.Id == 100);
            Assert.NotNull(insertedEntity);
            Assert.Equal(2, insertedEntity.Id);
            Assert.Null(notFoundEntity);
        }

        [Fact]
        [WithRecreatingTable]
        public async Task GetPageAsyncPredicates()
        {
            var sort = new List<ISort>()
            {  
                Predicates.Sort<TestEntity>(x => x.Id, false) 
            };

            int res1 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 1, Name = "1" });
            int res2 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 2, Name = "2" });
            int res3 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 3, Name = "3" });
            int res4 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 4, Name = "4" });
            int res5 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 5, Name = "3" });
            int res6 = await _fixture.CrudService.InsertAsync(new TestEntity { Id = 6, Name = "6" });

            var second = (await _fixture.Repository.GetPageAsync<TestEntity>(x => x.Id == 2, sort, 0, 10)).Single();
            var fifth = (await _fixture.Repository.GetPageAsync<TestEntity>(x => x.Id == 5 && x.Name == "3", sort, 0, 10)).Single();
            var thirdAndFifth = (await _fixture.Repository.GetPageAsync<TestEntity>(x => x.Name == "3", sort, 0, 10)).ToList();
            Assert.Equal(2, second.Id);
            Assert.Equal(5, fifth.Id);
            Assert.Equal(2, thirdAndFifth.Count);
            Assert.Equal(5, thirdAndFifth.First().Id);
            Assert.Equal(3, thirdAndFifth.Skip(1).First().Id);
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities(6)]
        public async Task GetPaginatedResultAsync()
        {
            var sort = new List<ISort>()
            {
                Predicates.Sort<TestEntity>(x => x.Id)
            };

            var firstPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(null, sort, 0, 2);
            var secondPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(null, sort, 1, 2);
            var thirdPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(null, sort, 2, 2);

            Assert.Equal(6, firstPage.Total);
            Assert.Equal(2, firstPage.Data.Count());
            Assert.Equal(1, firstPage.Data.First().Id);
            Assert.Equal(2, firstPage.Data.Skip(1).First().Id);

            Assert.Equal(6, secondPage.Total);
            Assert.Equal(2, secondPage.Data.Count());
            Assert.Equal(3, secondPage.Data.First().Id);
            Assert.Equal(4, secondPage.Data.Skip(1).First().Id);

            Assert.Equal(6, thirdPage.Total);
            Assert.Equal(2, thirdPage.Data.Count());
            Assert.Equal(5, thirdPage.Data.First().Id);
            Assert.Equal(6, thirdPage.Data.Skip(1).First().Id);
        }

        [Fact]
        [WithRecreatingTable]
        public async Task GetPaginatedResultAsyncByPredicate()
        {
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 1, Name = "X-Test01" });
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 2, Name = "X-Test02" });
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 3, Name = "X-Test03" });
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 4, Name = "Y-Test01" });
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 5, Name = "Y-Test02" });
            await _fixture.CrudService.InsertAsync(new TestEntity { Id = 6, Name = "Y-Test03" });

            var sort = new List<ISort>()
            {
                Predicates.Sort<TestEntity>(x => x.Id)
            };

            var firstPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 0, 2);
            var secondPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 1, 2);
            var thirdPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 2, 2);

            Assert.Equal(3, firstPage.Total);
            Assert.Equal(2, firstPage.Data.Count());
            Assert.Equal(1, firstPage.Data.First().Id);
            Assert.Equal(2, firstPage.Data.Skip(1).First().Id);

            Assert.Equal(3, secondPage.Total);
            Assert.Equal(3, Assert.Single(secondPage.Data).Id);

            Assert.Equal(3, thirdPage.Total);
            Assert.Empty(thirdPage.Data);
        }

        [Fact]
        [WithRecreatingTable]
        public async Task GetPaginatedResultAsyncByPredicateWithCustomConnection()
        {
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 1, Name = "X-Test01" });
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 2, Name = "X-Test02" });
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 3, Name = "X-Test03" });
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 4, Name = "Y-Test01" });
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 5, Name = "Y-Test02" });
                await _fixture.CrudService.InsertAsync(new TestEntity { Id = 6, Name = "Y-Test03" });

                var sort = new List<ISort>()
                {
                    Predicates.Sort<TestEntity>(x => x.Id)
                };

                var firstPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 0, 2, connection);
                var secondPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 1, 2, connection);
                var thirdPage = await _fixture.Repository.GetPaginatedResultAsync<TestEntity>(x => x.Name.Contains("X-Test"), sort, 2, 2, connection);

                Assert.Equal(3, firstPage.Total);
                Assert.Equal(2, firstPage.Data.Count());
                Assert.Equal(1, firstPage.Data.First().Id);
                Assert.Equal(2, firstPage.Data.Skip(1).First().Id);

                Assert.Equal(3, secondPage.Total);
                Assert.Equal(3, Assert.Single(secondPage.Data).Id);

                Assert.Equal(3, thirdPage.Total);
                Assert.Empty(thirdPage.Data);
            }
        }
    }
}
