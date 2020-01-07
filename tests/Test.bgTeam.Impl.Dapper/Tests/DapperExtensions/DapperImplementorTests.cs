using bgTeam.DataAccess;
using bgTeam.DataAccess.Impl.Dapper;
using Dapper;
using DapperExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.bgTeam.Impl.Dapper.Common;
using Test.bgTeam.Impl.Dapper.Common.Attributes;
using Xunit;
using static DapperExtensions.GetMultiplePredicate;

namespace Test.bgTeam.Impl.Dapper.Tests.DapperExtensions
{
    [Collection("SqlLiteCollection")]
    public class DapperImplementorTests
    {
        private static SqlLiteFixture _fixture;

        public DapperImplementorTests(SqlLiteFixture crudServiceFixture)
        {
            _fixture = crudServiceFixture;
        }

        [Fact]
        [WithRecreatingTable]
        public async Task InsertAndGetAsync()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                dapper.Insert(connection, new List<TestEntity> { new TestEntity() }, null, null);
                var result = await dapper.GetAsync<TestEntity>(connection, 1, null, null);
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetAsyncWithCustomObject()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var result = await dapper.GetAsync<TestEntity>(connection, new { Id = 1 }, null, null);
                Assert.NotNull(result);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task Delete()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                dapper.Delete(connection, new TestEntity() { Id = 1 }, null, null);
                Assert.Null(await dapper.GetAsync<TestEntity>(connection, 1, null, null));

                Assert.Throws<ArgumentException>(() => dapper.Delete(connection, new TestClass(), null, null));
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task DeleteWithPredicate()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var predicate = new FieldPredicate<TestEntity>
                {
                    Operator = Operator.Eq,
                    PropertyName = "Id",
                    Value = 1
                };
                dapper.Delete<TestEntity>(connection, predicate, null, null);
                Assert.Null(await dapper.GetAsync<TestEntity>(connection, 1, null, null));
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task DeleteWithCustomObject()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                dapper.Delete<TestEntity>(connection, new { Id = 1, Name = "1" }, null, null);
                Assert.Null(await dapper.GetAsync<TestEntity>(connection, 1, null, null));
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetSet()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            var sort = new List<ISort>()
            {
                Predicates.Sort<TestEntity>(x => x.Id)
            };
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var result = dapper.GetSet<TestEntity>(connection, null, sort, 1, 2, null, null, true);
                Assert.Equal(2, result.Count());
                Assert.Equal(2, result.First().Id);
                Assert.Equal(3, result.Last().Id);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task Count()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var predicate = new FieldPredicate<TestEntity> 
                { 
                    Operator = Operator.Gt, 
                    PropertyName = "Id", 
                    Value = 1 
                };
                var result = dapper.Count<TestEntity>(connection, predicate, null, null);
                Assert.Equal(2, result);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetMultiple()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var predicate = new GetMultiplePredicate();
                predicate.Add<TestEntity>(new FieldPredicate<TestEntity>
                {
                    Operator = Operator.Gt,
                    PropertyName = "Id",
                    Value = 1
                });
                predicate.Add<TestEntity>(new FieldPredicate<TestEntity>
                {
                    Operator = Operator.Eq,
                    PropertyName = "Name",
                    Value = "2"
                });

                var result = dapper.GetMultiple(connection, predicate, null, null);

                var result1 = result.Read<TestEntity>();
                Assert.Equal(2, result1.Count());

                var result2 = result.Read<TestEntity>();
                Assert.Single(result2);
            }
        }

        [Fact]
        [WithRecreatingTable]
        [InsertTestEntities]
        public async Task GetMultipleBySequence()
        {
            var dapper = new DapperImplementor(SqlHelper.GetSqlGenerator());
            using (var connection = await _fixture.Factory.ConnectionFactory.CreateAsync())
            {
                var predicate = new GetMultiplePredicate();
                predicate.Add<TestEntity>(new FieldPredicate<TestEntity>
                {
                    Operator = Operator.Gt,
                    PropertyName = "Id",
                    Value = 1
                });
                predicate.Add<TestEntity>(new FieldPredicate<TestEntity>
                {
                    Operator = Operator.Eq,
                    PropertyName = "Name",
                    Value = "2"
                });


                var result = dapper.GetMultiple(connection, predicate, null, null);

                var result1 = result.Read<TestEntity>();
                Assert.Equal(2, result1.Count());

                var result2 = result.Read<TestEntity>();
                Assert.Single(result2);
            }
        }

        class TestClass
        { 
        }
    }
}
